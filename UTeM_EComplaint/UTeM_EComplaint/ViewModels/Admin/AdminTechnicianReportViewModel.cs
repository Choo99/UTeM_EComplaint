using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace UTeM_EComplaint.ViewModels
{
    internal class AdminTechnicianReportViewModel : ViewModelBase
    {
        readonly int LOAD_SIZE = 5;

        bool isLoading;

        List<Technician> technicians;

        string selectedYear;

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public string SelectedYear { get => selectedYear; set { SetProperty(ref selectedYear, value); getData(); } }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand DownloadCommand { get; }

        public ObservableRangeCollection<Technician> TechnicianList { get; }
        public ObservableRangeCollection<string> YearList { get; }
        public AdminTechnicianReportViewModel()
        {
            TechnicianList = new ObservableRangeCollection<Technician>();
            YearList = new ObservableRangeCollection<string>();
            YearList.AddRange(new string[]
            {
                DateTime.Now.AddYears(-1).Date.Year.ToString(),
                DateTime.Now.AddYears(0).Date.Year.ToString(),
            });

            SelectedYear = DateTime.Now.Date.Year.ToString();

            technicians = new List<Technician>();

            LoadMoreCommand = new AsyncCommand(LoadMore);
            RefreshCommand = new AsyncCommand(Refresh);
            DownloadCommand = new AsyncCommand(Download);

            getData();
        }

        private async Task LoadMore()
        {
            if (TechnicianList.Count == technicians.Count)
                return;

            int lastItemIndexed = TechnicianList.Count;
            int nextItemIndexed = lastItemIndexed + LOAD_SIZE;

            if (nextItemIndexed > technicians.Count)
                nextItemIndexed = technicians.Count;

            for (int i = lastItemIndexed; i < nextItemIndexed; i++)
            {
                TechnicianList.Add(technicians[i]);
            }
            await Task.Delay(1000);
        }

        async Task Refresh()
        {
            await Task.Delay(1000);
            IsBusy = true;
            getData();
            IsBusy = false;
        }

        async void getData()
        {
            try
            {
                int size = LOAD_SIZE;
                IsLoading = true;
                technicians = await TechnicianServices.GetAllTechnicianWithStatisticByYear(SelectedYear);
                if (technicians.Count < LOAD_SIZE)
                    size = technicians.Count;
                TechnicianList.ReplaceRange(technicians.GetRange(0, size));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task Download()
        {
            IsLoading = true;
            await Generate();
            IsLoading = false;
        }

        private async Task Generate()
        {
            List<Technician> technicians = null;
            try
            {
                List<object> complaintObjs = new List<object>();

                int totalHeight = 0;

                technicians = await TechnicianServices.GetAllTechnicianWithStatisticByYear(selectedYear);

                PdfDocument doc = new PdfDocument();
                PdfPage page = doc.Pages.Add();

                PdfGraphics graphics = page.Graphics;

                var assembly = this.GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("UTeM_EComplaint.UTeM.png");

                PdfImage image = PdfBitmap.FromStream(stream);

                //get the half of page width and minus half of image width
                var imageStartPoint = page.GetClientSize().Width / 2 - (image.Width / 2) / 2;
                graphics.DrawImage(image, new RectangleF(imageStartPoint, 0, image.Width / 2, image.Height / 2));
                totalHeight += (image.Height / 2);

                PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);

                string text = String.Format("Technician Record From 1 January {0} to 31 December {1}",selectedYear,selectedYear);

                SizeF size = font.MeasureString(text);

                PdfStringFormat titleFormat = new PdfStringFormat();
                titleFormat.Alignment = PdfTextAlignment.Center;

                graphics.DrawString(text, font, PdfBrushes.Black, new RectangleF(0, totalHeight + 20, page.GetClientSize().Width, size.Height), titleFormat);
                totalHeight += (int)size.Height;

                PdfGrid pdfGrid = new PdfGrid();
                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("Technician ID");
                dataTable.Columns.Add("Name");
                dataTable.Columns.Add("Overall Rating");
                
                pdfGrid.Headers.Add(1);
                PdfGridRow pdfGridHeader = pdfGrid.Headers[0];

                

                foreach (Technician technician in technicians)
                {
                    dataTable.Rows.Add(new string[]
                    {
                        technician.TechnicianID.ToString(),
                        technician.TechnicianName,
                        technician.OverallRating.ToString(),
                    });

                }
                pdfGrid.DataSource = dataTable;

                PdfGridCellStyle pdfGridCellStyle = new PdfGridCellStyle();
                pdfGridCellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Bold);
                pdfGrid.Headers.ApplyStyle(pdfGridCellStyle);

                //create and customize the string formats
                PdfStringFormat stringFormat = new PdfStringFormat();
                stringFormat.Alignment = PdfTextAlignment.Center;

                for (int i = 0; i < pdfGrid.Columns.Count; i++)
                {
                    pdfGrid.Headers[0].Cells[i].StringFormat = stringFormat;
                }

                for (int i = 0; i < pdfGrid.Rows.Count; i++)
                {
                    //pdfGrid.Headers[0].Cells[i].StringFormat = stringFormat;
                    for (int j = 0; j < pdfGrid.Columns.Count; j++)
                    {
                        pdfGrid.Rows[i].Cells[j].StringFormat = stringFormat;
                    }
                }

                PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
                layoutFormat.Break = PdfLayoutBreakType.FitPage;
                layoutFormat.Layout = PdfLayoutType.Paginate;

                pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.PlainTable1);
                PdfGridLayoutResult pdfGridLayoutResult = pdfGrid.Draw(page, new PointF(0, totalHeight + 50), layoutFormat);

                string time = DateTimeOffset.Now.ToString("yyyy/MM//dd hh:mm:ss tt");
                graphics.DrawString("Generated At " + time, font, PdfBrushes.Black, new PointF(10, page.GetClientSize().Height - 20));

                MemoryStream memoryStream = new MemoryStream();

                doc.Save(memoryStream);

                //Close the document.

                doc.Close(true);

                //The operation in Save under Xamarin varies between Windows Phone, Android and iOS platforms. Please refer PDF/Xamarin section for respective code samples.
                await DependencyService.Get<ISave>().SaveAndView("Technician_Report_" + DateTimeOffset.Now.ToUnixTimeSeconds() + ".pdf", "application / pdf", memoryStream);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}
