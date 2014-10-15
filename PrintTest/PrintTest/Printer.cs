using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Printing;

namespace PrintTest
{
    public class Printer
    {
        #region Constructor & Properties

        public Printer()
        {
            this.PrintDocument = new PrintDocument();
            this.PrintDocumentSource = this.PrintDocument.DocumentSource;
            this.PrintManager = PrintManager.GetForCurrentView();
            this.PrintPreviewPages = new List<UIElement>();

            this.PrintDocument.Paginate += this.CreatePrintPreviewPages;
            this.PrintDocument.GetPreviewPage += this.GetPrintPreviewPage;
            this.PrintDocument.AddPages += this.AddPrintPages;
        }

        private PrintDocument PrintDocument { get; set; }

        private IPrintDocumentSource PrintDocumentSource { get; set; }

        private PrintManager PrintManager { get; set; }

        private List<UIElement> PrintPreviewPages { get; set; }

        #endregion Constructor & Properties

        public async Task Print()
        {
            this.RegisterForPrinting();
            await PrintManager.ShowPrintUIAsync();
            this.UnregisterForPrinting();
        }

        private List<UIElement> GetPreviewPages()
        {
            return new List<UIElement>()
            {
                new PrintPage(),
                new PrintPage(),
                new PrintPage(),
                new PrintPage(),
            };
        }

        #region Private Methods

        private void RegisterForPrinting()
        {
            this.PrintManager.PrintTaskRequested += this.PrintTaskRequested;
        }

        private void UnregisterForPrinting()
        {
            this.PrintManager.PrintTaskRequested -= this.PrintTaskRequested;
        }

        private void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            PrintTaskOptions printingOptions = ((PrintTaskOptions)e.PrintTaskOptions);

            PrintPageDescription pageDescription = printingOptions.GetPageDescription(0);

            this.PrintPreviewPages = this.GetPreviewPages();

            PrintDocument printDoc = (PrintDocument)sender;

            printDoc.SetPreviewPageCount(this.PrintPreviewPages.Count, PreviewPageCountType.Intermediate);
        }

        private void GetPrintPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            PrintDocument printDoc = (PrintDocument)sender;
            printDoc.SetPreviewPage(e.PageNumber, this.PrintPreviewPages[e.PageNumber - 1]);
        }

        private void AddPrintPages(object sender, AddPagesEventArgs e)
        {
            for (int i = 0; i < this.PrintPreviewPages.Count; i++)
            {
                this.PrintDocument.AddPage(this.PrintPreviewPages[i]);
            }

            PrintDocument printDoc = (PrintDocument)sender;
            printDoc.AddPagesComplete();
        }

        private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask("Print Test Task", sourceRequested =>
            {
                sourceRequested.SetSource(this.PrintDocumentSource);
            });
        }

        #endregion Private Methods
    }
}