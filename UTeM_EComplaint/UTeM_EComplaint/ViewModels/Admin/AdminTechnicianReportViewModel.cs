using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

        public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }
        public AsyncCommand LoadMoreCommand { get; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand DownloadCommand { get; }

        public ObservableRangeCollection<Technician> TechnicianList { get; }
        public AdminTechnicianReportViewModel()
        {
            TechnicianList = new ObservableRangeCollection<Technician>();
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
                IsBusy = true;
                technicians = await TechnicianServices.GetAllTechnicianWithStatistic();
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
                IsBusy = false;
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


                technicians = await TechnicianServices.GetAllTechnicianWithStatistic();

                PdfDocument doc = new PdfDocument();
                PdfPage page = doc.Pages.Add();

                PdfGraphics graphics = page.Graphics;
                PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);

                string text = "Technician Record";

                SizeF size = font.MeasureString(text);

                PdfStringFormat titleFormat = new PdfStringFormat();
                titleFormat.Alignment = PdfTextAlignment.Center;
                graphics.DrawString(text, font, PdfBrushes.Black, new PointF(10, 0));

                PdfGrid pdfGrid = new PdfGrid();
                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("Technician ID");
                dataTable.Columns.Add("Name");
                dataTable.Columns.Add("Overall Rating");
                dataTable.Columns.Add("Number of Rating");
                dataTable.Columns.Add("Completed Task");

                pdfGrid.Headers.Add(1);
                PdfGridRow pdfGridHeader = pdfGrid.Headers[0];

                foreach (Technician technician in technicians)
                {
                    dataTable.Rows.Add(new string[]
                    {
                        technician.TechnicianID.ToString(),
                        technician.TechnicianName,
                        technician.OverallRating.ToString(),
                        technician.CountRating.ToString(),
                        technician.CompletedTask.ToString(),
                    });

                }
                pdfGrid.DataSource = dataTable;

                //create and customize the string formats
                PdfStringFormat stringFormat = new PdfStringFormat();
                stringFormat.Alignment = PdfTextAlignment.Center;

                for (int i = 0; i < pdfGrid.Columns.Count; i++)
                {
                    pdfGrid.Headers[0].Cells[i].StringFormat = stringFormat;
                }

                PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
                layoutFormat.Break = PdfLayoutBreakType.FitPage;
                layoutFormat.Layout = PdfLayoutType.Paginate;

                pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.PlainTable1);
                PdfGridLayoutResult pdfGridLayoutResult = pdfGrid.Draw(page, new PointF(10, size.Height + 10), layoutFormat);

                string time = DateTimeOffset.Now.ToString("yyyy/MM//dd hh:mm:ss tt");
                graphics.DrawString("Generated At " + time, font, PdfBrushes.Black, new PointF(10, pdfGridLayoutResult.Bounds.Bottom + 20));

                MemoryStream stream = new MemoryStream();

                doc.Save(stream);

                //Close the document.

                doc.Close(true);

                //The operation in Save under Xamarin varies between Windows Phone, Android and iOS platforms. Please refer PDF/Xamarin section for respective code samples.
                await DependencyService.Get<ISave>().SaveAndView("Technician_Report_" + DateTimeOffset.Now.ToUnixTimeSeconds() + ".pdf", "application / pdf", stream);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}
