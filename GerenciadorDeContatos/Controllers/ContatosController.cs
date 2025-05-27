using GerenciadorDeContatos.Models;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorDeContatos.Controllers
{
    public class ContatosController : Controller
    {
        public static List<Contatos> GerarListaContato = new List<Contatos>();

        private void AddContato()
        {
            if(!GerarListaContato.Any())

            {
                GerarListaContato.AddRange(new List<Contatos>
                {
                    new Contatos { Id = 1, Nome = "Alice", Email = "alice@example.com", Telefone = "123456789", DataNascimento = new DateTime(1990, 1, 1) },
                    new Contatos { Id = 2, Nome = "Bob", Email = "bob@example.com", Telefone = "987654321", DataNascimento = new DateTime(1985, 5, 15) },
                    new Contatos { Id = 3, Nome = "Carlos", Email = "carlos@example.com", Telefone = "555123456", DataNascimento = new DateTime(1992, 9, 30) }
                });

            }
        }
        
        public IActionResult Index()
        {
            AddContato();
            return View(GerarListaContato);
        }

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


    }
}
