using System;
using System.IO;
using System.Linq;
using PdfSharp.Pdf; // Biblioteca PDF Sharp.
using PdfSharp.Pdf.IO; // Biblioteca PDF Sharp.
using PdfSharp.Pdf.Content; // Biblioteca PDF Sharp.


class ExtractPDFToText
{
    static void Main() // Função Principal para acessar os arquivos e pastas.
    {

        string PastaOrigem = "C:\\Users\\Sabriel Sacramento\\OneDrive\\Área de Trabalho\\pdf\\origem";
        string PastaDestino = "C:\\Users\\Sabriel Sacramento\\OneDrive\\Área de Trabalho\\pdf\\destino";
        string pastaLogs = "C:\\Users\\Sabriel Sacramento\\OneDrive\\Área de Trabalho\\pdf\\logs";


        while(true) 
        {
            string[] arquivosPDF = Directory.GetFiles(PastaOrigem, "*.pdf");
            foreach (var caminhoPDF in arquivosPDF)
            {
                ProcessarArquivoPDF(caminhoPDF, PastaDestino, pastaLogs);
            }

            System.Threading.Thread.Sleep(5000); // A cada 5 segundos.
        }
    }

    static void ProcessarArquivoPDF(string caminhoPDF, string PastaDestino, string pastaLogs) // Processar Arq. - Extrair Arq. - Mover Arq. - Tratar caso dê erro.
    {                                                                                                    
        try
        {
            ExtratorTextoPDF extrator = new ExtratorTextoPDF();
            extrator.ExtrairTexto(caminhoPDF, Path.Combine(PastaDestino, Path.GetFileNameWithoutExtension(caminhoPDF) + ".txt"));
            MoverArquivoParaPasta(caminhoPDF, pastaLogs);
            Console.WriteLine($"Extração concluída para {caminhoPDF}. Movido para {pastaLogs}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao processar {caminhoPDF}: {ex.Message}");  
        }
    }

    static void MoverArquivoParaPasta(string CaminhoArquivo, string PastaDestino) // Mover para pasta DESTINO.
    {
        string NomeArquivo = Path.GetFileName(CaminhoArquivo);
        string destino = Path.Combine(PastaDestino, NomeArquivo);
        File.Move(CaminhoArquivo, destino);
    }
}

class ExtratorTextoPDF // Extrair texto do PDF na Pasta Destino.
{
    public void ExtrairTexto(string caminhoPDF, string caminhoTXT)
    {
        using (PdfDocument document = PdfReader.Open(caminhoPDF, PdfDocumentOpenMode.ReadOnly))
        {
            using (StreamWriter sw = new StreamWriter(caminhoTXT))
            {
                ProcessarPaginas(document, sw);
            }
        }
    }

    private void ProcessarPaginas(PdfDocument document, StreamWriter sw) // Processar páginas se o PDF possuir mais de uma.
    {
        foreach (var page in document.Pages)
        {
            string texto = ContentReader.ReadContent(page).ToString();
            EscreverNoArquivo(sw, texto);
        }
    }

    private void EscreverNoArquivo(StreamWriter sw, string texto) // Escrever no arquivo TXT gerado.
    {
        sw.WriteLine(texto);
    }

}