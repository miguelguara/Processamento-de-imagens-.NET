using System;
using System.Drawing;
using System.IO;
public class Program
{
static void Main()
    {

    //pega o caminho da imagem e o nome do arquivo separado
    string caminho = "Imagem/img.jpg";
    string nomeArquivo =  Path.GetFileNameWithoutExtension("Imagem/img.jpg");

    Bitmap original = Carregar_Imagem.CarregarImagem(caminho);

    //chamo as classes de aplicação dos filtros e salvo o bitmap alterado
    Bitmap cinza = Conversao_Cinza.ConverterParaCinza(original);
    cinza.Save($"SlyCooper/{nomeArquivo}_cinza.jpg");

    Bitmap sobel = Sobel.AplicarSobel(cinza);
    sobel.Save($"SlyCooper/{nomeArquivo}_sobel.jpg");
    
    Bitmap prewitt = Prewitt.AplicarPrewitt(cinza);
    prewitt.Save($"SlyCooper/{nomeArquivo}_prewitt.jpg");
    
    Bitmap laplaceImg = Laplace.AplicarLapace(cinza);
    laplaceImg.Save($"SlyCooper/{nomeArquivo}_laplace.jpg");

    }
}

//classe responsável por carregar a imagem e transformar em um Bitmap
class Carregar_Imagem
{
    public static Bitmap CarregarImagem(string caminho)
    {
        Bitmap imagem = new Bitmap(caminho);
        return imagem;
    }
}
//classe responsável por converter para cinza
class Conversao_Cinza
{
    public static Bitmap ConverterParaCinza(Bitmap img)
    {
        Bitmap nova = new Bitmap(img.Width, img.Height);

        //esses dois for é pra passar por todos os pixels da imagem
        for (int y = 0; y < img.Height; y++)
        {
            for (int x = 0; x < img.Width; x++)
            {
                //aqui ele pega a cor do pixel especificado para modificar
                Color pixel = img.GetPixel(x, y);

                //Aplicação da formula do cinza
                int cinza = (int)(0.299 * pixel.R +
                                  0.587 * pixel.G +
                                  0.114 * pixel.B);
                //aqui ele aplica o filtro cinza na imagem desejada
                nova.SetPixel(x, y, Color.FromArgb(cinza, cinza, cinza));
            }
        }
        //Retorna o bitmaip pronto e com o filtro aplicado
        return nova;
    }
}

class Prewitt
{
    public static Bitmap AplicarPrewitt(Bitmap img)
    {
        int[,] prewittX =
        {
            { -1, 0, 1 },
            { -1, 0, 1 },
            { -1, 0, 1 }
        };

        int[,] prewittY =
        {
            { -1, -1, -1 },
            {  0,  0,  0 },
            {  1,  1,  1 }
        };

        Bitmap nova = new Bitmap(img.Width, img.Height);

        for (int y = 1; y < img.Height - 1; y++)
        {
            for (int x = 1; x < img.Width - 1; x++)
            {
                int gx = 0;
                int gy = 0;

                for (int ky = -1; ky <= 1; ky++)
                {
                    for (int kx = -1; kx <= 1; kx++)
                    {
                        int pixel = img.GetPixel(x + kx, y + ky).R;

                        gx += pixel * prewittX[ky + 1, kx + 1];
                        gy += pixel * prewittY[ky + 1, kx + 1];
                    }
                }

                int magnitude = (int)Math.Sqrt(gx * gx + gy * gy);
                magnitude = Math.Clamp(magnitude, 0, 255);

                nova.SetPixel(x, y, Color.FromArgb(magnitude, magnitude, magnitude));
            }
        }

        return nova;
    }
}
//Classe de convolução generia para usar o laplace
class Laplace
{
 
    public static Bitmap AplicarLapace(Bitmap img)
    {
        //sem diagoais
        int[,] laplace = {
        { 0, -1, 0 },
        { -1, 4, -1 },
        { 0, -1, 0 }
        };

        Bitmap nova = new Bitmap(img.Width, img.Height);

        //aqui passa por todos os pixels da imagem
        for (int y = 1; y < img.Height - 1; y++)
        {
            for (int x = 1; x < img.Width - 1; x++)
            {
                int soma = 0;

                for (int ky = -1; ky <= 1; ky++)
                {
                    for (int kx = -1; kx <= 1; kx++)
                    {
                        int pixel = img.GetPixel(x + kx, y + ky).R;
                        soma += pixel * laplace[ky + 1, kx + 1];
                    }
                }

                soma = Math.Clamp(Math.Abs(soma), 0, 255);

                nova.SetPixel(x, y, Color.FromArgb(soma, soma, soma));
            }
        }

        return nova;
    }
}

//Classe responsável por aplicar o filtro de Sobel
class Sobel
{
    public static Bitmap AplicarSobel(Bitmap img)
    {
    //matrizes de sobel
    int[,] sobelX = {
    { -1, 0, 1 },
    { -2, 0, 2 },
    { -1, 0, 1 }
    };

    int[,] sobelY = {
    { -1, -2, -1 },
    {  0,  0,  0 },
    {  1,  2,  1 }
    };

        Bitmap nova = new Bitmap(img.Width, img.Height);

        for (int y = 1; y < img.Height - 1; y++)
        {
            for (int x = 1; x < img.Width - 1; x++)
            {
                int gx = 0;
                int gy = 0;

                for (int ky = -1; ky <= 1; ky++)
                {
                    for (int kx = -1; kx <= 1; kx++)
                    {
                        int pixel = img.GetPixel(x + kx, y + ky).R;

                        gx += pixel * sobelX[ky + 1, kx + 1];
                        gy += pixel * sobelY[ky + 1, kx + 1];
                    }
                }

                int magnitude = (int)Math.Sqrt(gx * gx + gy * gy);
                magnitude = Math.Clamp(magnitude, 0, 255);

                nova.SetPixel(x, y, Color.FromArgb(magnitude, magnitude, magnitude));
            }
        }

        return nova;
    }
}
