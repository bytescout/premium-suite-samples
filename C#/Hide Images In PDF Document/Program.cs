//*******************************************************************************************//
//                                                                                           //
// Download Free Evaluation Version From: https://bytescout.com/download/web-installer       //
//                                                                                           //
// Also available as Web API! Get Your Free API Key: https://app.pdf.co/signup               //
//                                                                                           //
// Copyright © 2017-2019 ByteScout, Inc. All rights reserved.                                //
// https://www.bytescout.com                                                                 //
// https://pdf.co                                                                            //
//                                                                                           //
//*******************************************************************************************//


using System.Diagnostics;
using System.Drawing;
using Bytescout.PDF;
using Bytescout.PDFExtractor;
using Image = Bytescout.PDF.Image;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPdfDocument = @".\sample.pdf";
            string maskingImage = @".\mask.png";
            string resultPdfDocument = @".\result.pdf";


            // Create Bytescout.PDFExtractor.ImageExtractor instance
            using (ImageExtractor imageExtractor = new ImageExtractor())
            {
                imageExtractor.RegistrationName = "demo";
                imageExtractor.RegistrationKey = "demo";

                // Load document
                imageExtractor.LoadDocumentFromFile(inputPdfDocument);

                // Create Bytescout.PDF.Document instance
                using (Document pdfDocument = new Document())
                {
                    pdfDocument.RegistrationName = "demo";
                    pdfDocument.RegistrationKey = "demo";

                    // Load document
                    pdfDocument.Load(inputPdfDocument);
                    
                    // Load masking image
                    var maskImage = new Image(maskingImage);

                    // Process pages
                    for (int i = 0; i < pdfDocument.Pages.Count; i++)
                    {
                        Page page = pdfDocument.Pages[i];

                        // Find images on the page
                        if (imageExtractor.GetFirstPageImage(i))
                        {
                            do
                            {
                                // Get image rectangle
                                var imageRect = imageExtractor.GetCurrentImageRectangle();
                                
                                // Draw new image other the found image
                                page.Canvas.DrawImage(maskImage, imageRect.Left, imageRect.Top, imageRect.Width, imageRect.Height);

                            } while (imageExtractor.GetNextImage());
                        }
                    }

                    // Save modified document
                    pdfDocument.Save(resultPdfDocument);
                }
            }

            // Open document in default PDF viewer application (for demonstration).
            Process.Start(resultPdfDocument);
        }
    }
}
