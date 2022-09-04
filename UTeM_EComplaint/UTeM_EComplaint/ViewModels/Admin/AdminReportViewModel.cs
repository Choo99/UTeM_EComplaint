using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Tables;
using UTeM_EComplaint.Model;
using UTeM_EComplaint.Services;
using UTeM_EComplaint.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UTeM_EComplaint.ViewModels
{
    internal class AdminReportViewModel : ViewModelBase
    {
        DateTime startDate;
        DateTime endDate;
        DateTime minDate;
        DateTime maxDate;

        DateTime oneMonthStartTime;
        DateTime twoMonthStartTime;
        DateTime threeMonthStartTime;
        DateTime monthlyEndTime;

        ComplaintType selectedComplaintType;
        public ComplaintType SelectedComplaintType { get => selectedComplaintType; set => SetProperty(ref selectedComplaintType, value); }
        public ObservableRangeCollection<ComplaintType> ComplaintTypeList { get; }

        public DateTime OneMonthStartTime { get => oneMonthStartTime; set => oneMonthStartTime = value; }

        public DateTime TwoMonthStartTime { get => twoMonthStartTime; set => twoMonthStartTime = value; }

        public DateTime ThreeMonthStartTime { get => threeMonthStartTime; set => threeMonthStartTime = value; }
        public DateTime MonthlyEndTime { get => monthlyEndTime; set => monthlyEndTime = value; }

        public DateTime MinDate { get => minDate; set => minDate = value; }
        public DateTime MaxDate { get => maxDate; set => maxDate = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }

        public AsyncCommand GenerateSpecificCommand { get; }
        public AsyncCommand GenerateOneMonthCommand { get; }
        public AsyncCommand GenerateTwoMonthCommand { get; }
        public AsyncCommand GenerateThreeMonthCommand { get; }
        public AdminReportViewModel()
        {
            Title = "Generate Report";
            GenerateSpecificCommand = new AsyncCommand(GenerateSpecific);
            GenerateOneMonthCommand = new AsyncCommand(GenerateOneMonth);
            GenerateTwoMonthCommand = new AsyncCommand(GenerateTwoMonth);
            GenerateThreeMonthCommand = new AsyncCommand(GenerateThreeMonth);

            OneMonthStartTime = DateTime.Now.AddMonths(-1);
            TwoMonthStartTime = DateTime.Now.AddMonths(-2);
            ThreeMonthStartTime = DateTime.Now.AddMonths(-3);

            monthlyEndTime = DateTime.Now;

            ComplaintTypeList = new ObservableRangeCollection<ComplaintType>();
            getData();

            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            MinDate = DateTime.Now.AddMonths(-12);
            MaxDate = DateTime.Now;
        }

        private async void getData()
        {
            ComplaintTypeList.ReplaceRange(await ComplaintTypeServices.GetComplaintType());
        }

        private async Task GenerateSpecific()
        {
            IsBusy = true;
            if ((startDate.CompareTo(endDate) < 0 && endDate.CompareTo(startDate) > 0) || endDate.CompareTo(startDate) == 0)
                await GenerateByDate(startDate, endDate);
            else if (startDate.CompareTo(endDate) > 0)
                await Application.Current.MainPage.DisplayAlert("Date", "End date cannot be less than start date", "OK");
            else
                await Application.Current.MainPage.DisplayAlert("Date", "Something wrong happen", "OK");
            IsBusy = false;
        }
        private async Task GenerateOneMonth()
        {
            IsBusy = true;
            await GenerateByDate(OneMonthStartTime, monthlyEndTime);
            IsBusy = false;
        }
        private async Task GenerateTwoMonth()
        {
            IsBusy = true;
            await GenerateByDate(TwoMonthStartTime, monthlyEndTime);
            IsBusy = false;
        }
        private async Task GenerateThreeMonth()
        {
            IsBusy = true;
            await GenerateByDate(ThreeMonthStartTime, monthlyEndTime);
            IsBusy = false;
        }

        private async Task GenerateByDate(DateTime startDate, DateTime endDate)
        {
            
            try
            {
                if(selectedComplaintType.ComplaintTypeCode == "H")
                    GenerateHardwareReport(startDate,endDate);
                else
                    GenerateSoftwareReport(startDate, endDate);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }

        private async void GenerateHardwareReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<Complaint> complaints = null;
                List<object> complaintObjs = new List<object>();


                complaints = await ComplaintServices.GetComplaintsByDate(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                if (complaints.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("No result", "No result found between this period", "OK");
                    return;
                }

                int totalHeight = 0;

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

                string text = "Complaint Record from " + startDate.ToString("yyyy-MM-dd") + " - " + endDate.ToString("yyyy-MM-dd");

                SizeF size = font.MeasureString(text);

                PdfStringFormat titleFormat = new PdfStringFormat();
                titleFormat.Alignment = PdfTextAlignment.Center;

                graphics.DrawString(text, font, PdfBrushes.Black, new RectangleF(0, totalHeight + 20, page.GetClientSize().Width, size.Height), titleFormat);
                totalHeight += (int)size.Height;

                PdfGrid pdfGrid = new PdfGrid();
                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("Date");
                dataTable.Columns.Add("Complaint ID");
                dataTable.Columns.Add("Staff ID");
                dataTable.Columns.Add("Staff Name");
                dataTable.Columns.Add("Category");
                dataTable.Columns.Add("Division");
                dataTable.Columns.Add("DamageType");
                dataTable.Columns.Add("Description");
                dataTable.Columns.Add("Location");
                dataTable.Columns.Add("Contact Number");
                dataTable.Columns.Add("Complaint Status");

                pdfGrid.Headers.Add(1);
                PdfGridRow pdfGridHeader = pdfGrid.Headers[0];

                foreach (Complaint complaint in complaints)
                {
                    dataTable.Rows.Add(new string[]
                    {
                        complaint.ComplaintDate,
                        complaint.ComplaintID.ToString(),
                        complaint.Staff.StaffID.ToString(),
                        complaint.Staff.Name,
                        complaint.Category.CategoryName,
                        complaint.Division.DivisionName,
                        complaint.DamageType.DamageTypeName,
                        complaint.Damage,
                        complaint.Location,
                        complaint.ContactPhoneNumber,
                        complaint.ComplaintStatus
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
                PdfGridLayoutResult pdfGridLayoutResult = pdfGrid.Draw(page, new PointF(0, totalHeight + 50), layoutFormat);
                totalHeight += (int)pdfGridLayoutResult.Bounds.Height;



                string time = DateTimeOffset.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
                pdfGridLayoutResult.Page.Graphics.DrawString("Generated At " + time, font, PdfBrushes.Black, new PointF(0, page.GetClientSize().Height - 20));

                MemoryStream memoryStream = new MemoryStream();

                doc.Save(memoryStream);

                //Close the document.

                doc.Close(true);

                //The operation in Save under Xamarin varies between Windows Phone, Android and iOS platforms. Please refer PDF/Xamarin section for respective code samples.
                await DependencyService.Get<ISave>().SaveAndView("Complaint_Date_" + DateTimeOffset.Now.ToUnixTimeSeconds() + ".pdf", "application / pdf", memoryStream);
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
            
        }

        private async void GenerateSoftwareReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<Complaint> complaints = null;
                List<object> complaintObjs = new List<object>();


                complaints = await ComplaintServices.GetSoftwareComplaintsByDate(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                if (complaints.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("No result", "No result found between this period", "OK");
                    return;
                }

                int totalHeight = 0;

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

                string text = "Complaint Record from " + startDate.ToString("yyyy-MM-dd") + " - " + endDate.ToString("yyyy-MM-dd");

                SizeF size = font.MeasureString(text);

                PdfStringFormat titleFormat = new PdfStringFormat();
                titleFormat.Alignment = PdfTextAlignment.Center;

                graphics.DrawString(text, font, PdfBrushes.Black, new RectangleF(0, totalHeight + 20, page.GetClientSize().Width, size.Height), titleFormat);
                totalHeight += (int)size.Height;

                PdfGrid pdfGrid = new PdfGrid();
                DataTable dataTable = new DataTable();

                dataTable.Columns.Add("Date");
                dataTable.Columns.Add("Complaint ID");
                dataTable.Columns.Add("Staff ID");
                dataTable.Columns.Add("Staff Name");
                dataTable.Columns.Add("System");
                dataTable.Columns.Add("Module");
                dataTable.Columns.Add("Submodule");
                dataTable.Columns.Add("Submenu");
                dataTable.Columns.Add("Description");
                dataTable.Columns.Add("Contact Number");
                dataTable.Columns.Add("Complaint Status");

                pdfGrid.Headers.Add(1);
                PdfGridRow pdfGridHeader = pdfGrid.Headers[0];

                foreach (Complaint complaint in complaints)
                {
                    dataTable.Rows.Add(new string[]
                    {
                        complaint.ComplaintDate,
                        complaint.ComplaintID,
                        complaint.Staff.StaffID.ToString(),
                        complaint.Staff.Name,
                        complaint.SoftwareSystem.DisplaySoftwareSystem,
                        complaint.Module.DisplayModule,
                        complaint.Submodule.DisplaySubmodule,
                        complaint.Submenu.DisplaySubmenu,
                        complaint.Damage,
                        complaint.ContactPhoneNumber,
                        complaint.ComplaintStatus
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
                PdfGridLayoutResult pdfGridLayoutResult = pdfGrid.Draw(page, new PointF(0, totalHeight + 50), layoutFormat);
                totalHeight += (int)pdfGridLayoutResult.Bounds.Height;



                string time = DateTimeOffset.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
                pdfGridLayoutResult.Page.Graphics.DrawString("Generated At " + time, font, PdfBrushes.Black, new PointF(0, page.GetClientSize().Height - 20));

                MemoryStream memoryStream = new MemoryStream();

                doc.Save(memoryStream);

                //Close the document.

                doc.Close(true);

                //The operation in Save under Xamarin varies between Windows Phone, Android and iOS platforms. Please refer PDF/Xamarin section for respective code samples.
                await DependencyService.Get<ISave>().SaveAndView("Complaint_Date_" + DateTimeOffset.Now.ToUnixTimeSeconds() + ".pdf", "application / pdf", memoryStream);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK");
            }
        }
    }
}
