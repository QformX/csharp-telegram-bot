using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenHandler
{
    public class Tokenizer
    {
        private string _path;

        public Tokenizer(string path)
        {
            _path = path;
        }

        public string Token()
        {
            var handler = new TokenGenerator();
            var token = handler.GetToken(_path);
            return token;
        }
    }
}
