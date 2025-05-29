using GerenciadorDeContatos.Models;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace GerenciadorDeContatos.Controllers
{
    public class ContatosController : Controller
    {
        public static List<Contatos> GerarListaContato = new List<Contatos>();

        private void AddContato()
        {
            if (!GerarListaContato.Any())

            {
                GerarListaContato.AddRange(new List<Contatos>
                {
                    new Contatos { Id = 1, Nome = "Alice Maravilha", Email = "alice@gmail.com", Telefone = "(12) 34567-8975", Endereco="Rua do Zé, 254", DataNascimento = new DateTime(1990, 1, 1) , Categoria = "Amigo" },
                    new Contatos { Id = 2, Nome = "Lúcio Nemo", Email = "Lucio@gmail.com", Telefone = "(15) 99852-8597", Endereco="Rua Jururu, 170", DataNascimento = new DateTime(1998, 05, 28) , Categoria = "Professor" },
                    new Contatos { Id = 3, Nome = "Carla Naia", Email = "Carla@gmail.com", Telefone = "(12) 44520-5771", Endereco="Rua Gorgonzola, 05", DataNascimento = new DateTime(2005, 12, 25) , Categoria = "Família" },
                });

            }
        }

        [HttpGet]

        public IActionResult Index(string categoria)
        {
            AddContato();

            // Lista com as categorias principais
            var categoriasPrincipais = new List<string> { "Família", "Cliente", "Trabalho", "Amigo" };

            List<Contatos> contatos;

            if (string.IsNullOrEmpty(categoria) || categoria == "Todas")
            {
                contatos = GerarListaContato;
            }
            else if (categoria == "Outros")
            {
                // Pega os contatos cuja categoria não é uma das principais
                contatos = GerarListaContato
                    .Where(c => !categoriasPrincipais.Contains(c.Categoria))
                    .ToList();
            }
            else
            {
                contatos = GerarListaContato
                    .Where(c => c.Categoria == categoria)
                    .ToList();
            }

            // Prepara as opções do filtro
            var categoriasFiltro = new List<string>(categoriasPrincipais)
            {
                "Outros"
            };

            ViewBag.Categorias = categoriasFiltro;

            //data de aniversário
            var mesAtual = DateTime.Now.Month;
            var aniversariantes = GerarListaContato.Where(c => c.DataNascimento.Month == mesAtual).ToList();

            ViewBag.Aniversariantes = aniversariantes;



            return View(contatos);
        }

        //Criar um novo contato
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contatos contato)
        {
            if (ModelState.IsValid)
            {
                contato.Id = GerarListaContato.Count + 1; // Simula um ID único
                GerarListaContato.Add(contato);
                return RedirectToAction("Index");
            }
            return View(contato);
        }

        //Editar um contato existente
        public IActionResult Edit(int id)
        {
            var contato = GerarListaContato.FirstOrDefault(c => c.Id == id);
            if (contato == null)
                return NotFound();

            return View(contato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Contatos contatoEditado)
        {
            var contato = GerarListaContato.FirstOrDefault(c => c.Id == contatoEditado.Id);
            if (contato == null)
                return NotFound();

            contato.Nome = contatoEditado.Nome;
            contato.Email = contatoEditado.Email;
            contato.Telefone = contatoEditado.Telefone;
            contato.Endereco = contatoEditado.Endereco;
            contato.DataNascimento = contatoEditado.DataNascimento;
            contato.Categoria = contatoEditado.Categoria;

            return RedirectToAction("Index");
        }

        // Detalhes de um contato

        public IActionResult Details(int id)
        {
            var contato = GerarListaContato.FirstOrDefault(c => c.Id == id);
            if (contato == null)
                return NotFound();
            return View(contato);
        }

        // Deletar um contato

        public IActionResult Delete(int id)
        {
            var contato = GerarListaContato.FirstOrDefault(c => c.Id == id);
            if (contato == null)
                return NotFound();

            return View(contato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection form)
        {
            var contato = GerarListaContato.FirstOrDefault(c => c.Id == id);
            if (contato != null)
            {
                GerarListaContato.Remove(contato);
            }
            return RedirectToAction("Index");

        }

        //exportar contatos para PDF
        public IActionResult ExportarParaPdf()
        {
            AddContato(); // Garante os dados
            var contatos = GerarListaContato;

            using (var ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 25, 25, 40, 25);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Fonte 
                var fonteTitulo = new Font(Font.FontFamily.HELVETICA, 20f, Font.BOLD, BaseColor.DARK_GRAY);
                var fonteCabecalho = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.WHITE);
                var fonteCelula = new Font(Font.FontFamily.HELVETICA, 11f, Font.NORMAL, BaseColor.BLACK);

                // Título
                Paragraph titulo = new Paragraph("Lista de Contatos", fonteTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingAfter = 20f;
                doc.Add(titulo);

                // Tabela com colunas
                PdfPTable tabela = new PdfPTable(6);
                tabela.WidthPercentage = 100;
                tabela.SetWidths(new float[] { 1, 2.5f, 3, 2.5f, 2, 2 });

                // Cores
                BaseColor corCabecalho = new BaseColor(255, 182, 193); // Rosa claro

                // Cabeçalhos
                string[] cabecalhos = { "ID", "Nome", "Email", "Telefone", "Nascimento", "Categoria" };
                foreach (var cab in cabecalhos)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(cab, fonteCabecalho));
                    cell.BackgroundColor = corCabecalho;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Padding = 8f;
                    tabela.AddCell(cell);
                }

                // Dados dos contatos
                foreach (var c in contatos)
                {
                    tabela.AddCell(new PdfPCell(new Phrase(c.Id.ToString(), fonteCelula)) { Padding = 5f });
                    tabela.AddCell(new PdfPCell(new Phrase(c.Nome, fonteCelula)) { Padding = 5f });
                    tabela.AddCell(new PdfPCell(new Phrase(c.Email, fonteCelula)) { Padding = 5f });
                    tabela.AddCell(new PdfPCell(new Phrase(c.Telefone, fonteCelula)) { Padding = 5f });
                    tabela.AddCell(new PdfPCell(new Phrase(c.DataNascimento.ToString("dd/MM/yyyy"), fonteCelula)) { Padding = 5f });
                    tabela.AddCell(new PdfPCell(new Phrase(string.IsNullOrEmpty(c.Categoria) ? "-" : c.Categoria, fonteCelula)) { Padding = 5f });
                }

                doc.Add(tabela);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "Contatos.pdf");
            }
        }


    }
}
