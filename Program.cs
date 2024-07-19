#region Library
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.Encodings;
using System.IO;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using project;
#endregion

namespace CadastroPessoa
{
    class Program
    {
        static async Task Main(string[] args)
        {
             UsuarioLogin dadosTelaPrincipal = new UsuarioLogin();
             dadosTelaPrincipal.TelaDeLogin();
             await dadosTelaPrincipal.CadastroLogin();

            Console.ReadLine();
        }
    }
}
