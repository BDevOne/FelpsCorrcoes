using CadastroPessoa;
using project.IntegracaoCep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace project
{
    public class UsuarioLogin
    {
        //Verificar possibilidade de implementar funcionalidade de cadastro de produtos por usuário.
        //Adicionar cadastro de produtos por usuário, usuário poderar optar por produtos ex: (Tenis, Blusa, calça, etc...)

        List<Users> usuariosValidos = new List<Users>();
        List<Users> usuariosInvalidos = new List<Users>();
        List<Users> listaCadastros = new List<Users>();

        private bool listaAcessada = false;

        public void TelaDeLogin()
        {
            Console.WriteLine("Bem vindo à Tela de Login\n");
            Console.WriteLine("\nDados Obrigátorios: Idade Ou Data de Nacismento/CPF\n\n");
        }

        public async Task CadastroLogin()
        {
            string seguirCadastrosUsuarios = "S";

            Random IdUsuario = new Random();

            // verificação caso o S for um valor nulo aguardar o valor correto, em andamento..
            while (!string.IsNullOrEmpty(seguirCadastrosUsuarios) && seguirCadastrosUsuarios.ToUpper() == "S")
            {
                //viaCepApi resultadoCep = new viaCepApi();
                Users cadastros = new Users();

                cadastros.Id = IdUsuario.Next(0, 80);
                Console.Write("\nNome do usuário: ");
                cadastros.Nome = Console.ReadLine();

                Console.Write("\nCPF do usuário: ");
                cadastros.Cpf = Console.ReadLine();

                Console.Write("\nData de nascimento do usuário: ");
                cadastros.DataDeNascimento = Console.ReadLine();
                cadastros.dataNascimentoCadastro();

                /* UsingCase
                Console.Write("\nInforme o CEP: ");
                cadastros.cep = Console.ReadLine();
                cadastros.Endereco = await resultadoCep.BuscarCepUsuario(cadastros.cep);
                */

                listaCadastros.Add(cadastros);

                Console.Write("\nDeseja cadastrar outros usuários? (S/N): ");
                seguirCadastrosUsuarios = Console.ReadLine();
            }

            handleUserAction();
            gerarListaCadastros(listaCadastros);
            ExibirDadosUsuariosCadastrados();
        }

        // Método Criar lista Cadastros
        public void gerarListaCadastros(List<Users> listaCadastros)
        {
            // Lista de Cadastros
            foreach (var user in listaCadastros)
            {
                // user.RemoverMascaraCpf();
                if (user.validacaoCpfUsuario() && user.validarIdadeUsuario())
                {
                    // adicionar validação de cadastro iguais pelo cpf
                    if (!usuariosValidos.Exists(u => u.Cpf == user.Cpf))
                    {
                        usuariosValidos.Add(user);
                    }
                    else
                    {
                        string cpfIgual = user.Cpf;
                        usuariosInvalidos.Add(user);
                        user.Cpf = $"CPF já cadastrado: {cpfIgual}";
                    }
                }
                else
                {
                    usuariosInvalidos.Add(user);
                }
            }
        }

        public void ExibirDadosUsuariosCadastrados()
        {
            Console.WriteLine($"\nDados dos Usuários\n");

            // Lista de Usuários cadastrados Válidos
            if (usuariosValidos.Count > 0) // Foi verificado que ao editar está sendo validado todo o processo novamente e isso esta duplicando o cadastro. -- Corrigido
            {
                Console.WriteLine("Usuários cadastrados");
                foreach (var user in usuariosValidos)
                {
                    Console.WriteLine($"\nID do Usuário: {user.Id}");
                    Console.WriteLine($"\nNome: {user.Nome}");
                    Console.WriteLine($"CPF: {user.Cpf}");

                    if (user.DataDeNascimento != null)
                    {
                        Console.WriteLine($"Data de Nascimento: {user.DataDeNascimento}");
                    }
                    Console.WriteLine($"Idade: {user.Idade}");
                }
            }
            else
            {
                Console.WriteLine("Nenhum Usuário cadastrado");
            }

            // Lista de Usuários não cadastrados
            if (usuariosInvalidos.Count > 0)
            {
                Console.WriteLine("\nUsuários não cadastrados");
                foreach (var user in usuariosInvalidos)
                {
                    Console.WriteLine($"\nNome: {user.Nome}");
                    if (user.Cpf.Length < 11 || user.validarIdadeUsuario())
                    {
                        Console.WriteLine("CPF: Não informado ou Incorreto.");
                    }
                    else
                    {
                        Console.WriteLine($"CPF: {user.Cpf}");
                    }
                    Console.WriteLine($"Data de Nascimento: {user.DataDeNascimento}");
                    Console.WriteLine($"Idade: {user.Idade}");
                }
            }
            else
            {
                Console.WriteLine("\nTodos Usuários Foram cadastrados corretamente.");
            }

            // Criar validacao por ID caso tenha nomes iguais, não exclui os demais com o mesmo nome.
        }

        // Criar método com a solicitação dos dados de usuario - Nome, CPF, Idade, Nascimento, etc...
        public void handleUserAction()
        {
            Console.WriteLine("Opções '1' / '2' / '3'");
            string action = Console.ReadLine();
            switch (action)
            {
                case "1":
                Console.WriteLine("Cadastrar Usuário");
                CadastroLogin();
                break;

                case "2":
                Console.WriteLine("Excluir Usuário");
                removerCadastros();
                break;

                case "3":
                Console.WriteLine("Editar Usuário");
                editarCadastro();
                break;

                default:
                Console.WriteLine("Deu bom não DOG");
                break;
            }
        }

        // Implementar também a verificação da lista de usuariosInvalidos para que seja possivel editar e torna-lo valido. -- Corrigido Aguardando correçaõ abaixo
        // Implemntar a edição de outras informações para que seja possível editar usuariosInvalidos.
        public void editarCadastro()
        {
            Console.Write("\nQual Usuário deseja editar ? ");
            string nomeAlterado = Console.ReadLine();
            foreach (var editarUsuario in listaCadastros)
            {
                Users usuarioAlterado = listaCadastros.FirstOrDefault(u => u.Nome.Equals(editarUsuario.Nome, StringComparison.OrdinalIgnoreCase));

                if (usuarioAlterado.Nome.Equals(nomeAlterado, StringComparison.OrdinalIgnoreCase))
                {
                    Console.Write("\nInforme o novo nome do usuário: ");
                    string novoNome = Console.ReadLine();
                    editarUsuario.Nome = novoNome;
                    break;
                }
            }
            Console.WriteLine("Lista de cadastros Atualizada");
            ExibirDadosUsuariosCadastrados();

        }
        
        // Adicionar validação pelo ID ao invés do nome, para que não seja excluido os cadastros com o mesmo usuário.
        public void removerCadastros()
        {
            Console.Write("\nQual Usuário deseja remover do cadastro? ");
            string usuarioRemovido = Console.ReadLine();

            foreach (var user in listaCadastros)
            {
                Users usuarioRemover = listaCadastros.FirstOrDefault(u => u.Nome.Equals(usuarioRemovido, StringComparison.OrdinalIgnoreCase));

                if (usuarioRemover.Nome.Equals(user.Nome, StringComparison.OrdinalIgnoreCase))
                {
                    listaCadastros.Remove(usuarioRemover);
                    Console.WriteLine($"\nUsuário {user.Nome} Removido!!\n");
                    break;
                }

            }
        }

        void ExibirErroCadastrarUsuario(string mensagemErro)
        {
            Console.WriteLine($"\n{mensagemErro}");
        }
    }
}
