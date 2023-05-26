using Newtonsoft.Json;
using OWNA_Test.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OWNA_Test.Persistence
{
    public class BookDataRepository
    {
        public List<Author> Authors;
        public List<Book> Books;

        public BookDataRepository()
        {
            var jsonData = File.ReadAllText("wwwroot/lib/Data/Book_Data.Json");

            var data = JsonConvert.DeserializeObject<AuthorBookData>(jsonData);

            Authors = data.Authors;
            Books = data.Books;

        }

        public List<Book> FindBooks(string search)
        {
            search = search.ToLower();

            var foundBooks = Books.Where(b =>
                                    b.Title.ToLower().Contains(search) ||
                                    b.Authors.Any(b => b.name.ToLower().Contains(search)))
                                    .ToList();

            return foundBooks;
        }

        public void AddBook(Book book)
        {
            int newID = Books.Max(b => b.Id) + 1;

            book.Id = newID;
            Books.Add(book);
        }

        public void AddAuthor(Author author)
        {
            int newID = Authors.Max(a => a.Id) + 1;

            author.Id = newID;
            Authors.Add(author);
        }
    }
}
