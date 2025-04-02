using System;
using System.Linq;

namespace JogoDosDados.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int limiteLinhaChegada = 30;
            Random geradorNumeros = new Random();

            while (true)
            {
                int posicaoUsuario = 0;
                int posicaoSistema = 0;
                bool jogoEmAndamento = true;

                Console.Clear();
                Console.WriteLine("----------------------------------");
                Console.WriteLine("        CORRIDA DE DADOS");
                Console.WriteLine("----------------------------------");
                Console.WriteLine($"Primeiro a chegar em {limiteLinhaChegada} vence!");
                Console.WriteLine("Eventos especiais:");
                Console.WriteLine("Posições 5,10,15: +3 casas");
                Console.WriteLine("Posições 7,13,20: -2 casas");
                Console.WriteLine("Tirar 6: rodada extra");
                Console.WriteLine("----------------------------------");
                Console.WriteLine("Pressione ENTER para começar...");
                Console.ReadLine();

                while (jogoEmAndamento)
                {
                    // Turno do Jogador
                    bool rodadaExtraJogador = false;
                    do
                    {
                        ExibirCabecalho("Jogador");
                        ExibirPista(posicaoUsuario, posicaoSistema, limiteLinhaChegada);

                        Console.WriteLine("Pressione ENTER para lançar o dado...");
                        Console.ReadLine();

                        int resultado = geradorNumeros.Next(1, 7);
                        ExibirResultadoSorteio(resultado);
                        posicaoUsuario += resultado;

                        // Verificar eventos da posição
                        int modificador = VerificarEventosPosicao(posicaoUsuario);
                        posicaoUsuario += modificador;
                        posicaoUsuario = Math.Max(0, posicaoUsuario);

                        // Verificar rodada extra
                        if (resultado == 6)
                        {
                            Console.WriteLine("\n>>> Você tirou 6! Ganhou uma rodada extra! <<<");
                            rodadaExtraJogador = true;
                        }
                        else
                        {
                            rodadaExtraJogador = false;
                        }

                        // Verificar vitória
                        if (posicaoUsuario >= limiteLinhaChegada)
                        {
                            Console.WriteLine("\nPARABÉNS! VOCÊ GANHOU!!!");
                            jogoEmAndamento = false;
                            break;
                        }

                        ExibirPista(posicaoUsuario, posicaoSistema, limiteLinhaChegada);

                        if (rodadaExtraJogador)
                        {
                            Console.WriteLine("Preparando sua rodada extra...");
                            Thread.Sleep(2000);
                        }

                    } while (rodadaExtraJogador && jogoEmAndamento);

                    if (!jogoEmAndamento) break;

                    // Turno do Sistema
                    bool rodadaExtraSistema = false;
                    do
                    {
                        ExibirCabecalho("Sistema");
                        ExibirPista(posicaoUsuario, posicaoSistema, limiteLinhaChegada);

                        Console.WriteLine("O sistema está lançando o dado...");
                        Thread.Sleep(1500);

                        int resultadoSistema = geradorNumeros.Next(1, 7);
                        ExibirResultadoSorteio(resultadoSistema);
                        posicaoSistema += resultadoSistema;

                        // Verificar eventos da posição
                        int modificador = VerificarEventosPosicao(posicaoSistema);
                        posicaoSistema += modificador;
                        posicaoSistema = Math.Max(0, posicaoSistema);

                        // Verificar rodada extra
                        if (resultadoSistema == 6)
                        {
                            Console.WriteLine("\n>>> O sistema tirou 6 e ganhou uma rodada extra! <<<");
                            rodadaExtraSistema = true;
                        }
                        else
                        {
                            rodadaExtraSistema = false;
                        }

                        // Verificar vitória
                        if (posicaoSistema >= limiteLinhaChegada)
                        {
                            Console.WriteLine("\nQUE PENA! O SISTEMA GANHOU!");
                            jogoEmAndamento = false;
                            break;
                        }

                        ExibirPista(posicaoUsuario, posicaoSistema, limiteLinhaChegada);

                        if (rodadaExtraSistema)
                        {
                            Console.WriteLine("O sistema está jogando sua rodada extra...");
                            Thread.Sleep(2000);
                        }

                    } while (rodadaExtraSistema && jogoEmAndamento);
                }

                string opContinuar = ExibirMenuContinuar();
                if (opContinuar != "S")
                    break;
            }
        }

        static void ExibirCabecalho(string nomeJogador)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------");
            Console.WriteLine("        CORRIDA DE DADOS");
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Turno do(a): {nomeJogador}\n");
        }

        static void ExibirResultadoSorteio(int resultado)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"    O valor sorteado foi: {resultado}");
            Console.WriteLine("----------------------------------\n");
        }

        static int VerificarEventosPosicao(int posicao)
        {
            int[] posicoesAvanco = { 5, 10, 15 };
            int[] posicoesRecuo = { 7, 13, 20 };

            if (posicoesAvanco.Contains(posicao))
            {
                Console.WriteLine("\n>>> Evento especial: Avanço extra de +3 casas! <<<");
                return 3;
            }

            if (posicoesRecuo.Contains(posicao))
            {
                Console.WriteLine("\n>>> Evento especial: Recuo de -2 casas! <<<");
                return -2;
            }

            return 0;
        }

        static void ExibirPista(int posicaoUsuario, int posicaoSistema, int limite)
        {
            Console.WriteLine("\nProgresso na pista (0 a {0}):", limite);

            for (int i = 0; i <= limite; i++)
            {
                if (i == posicaoUsuario && i == posicaoSistema && i != 0)
                    Console.Write("X");
                else if (i == posicaoUsuario)
                    Console.Write("J");
                else if (i == posicaoSistema)
                    Console.Write("S");
                else
                    Console.Write(".");
            }

            Console.WriteLine("\nLegenda: J = Jogador | S = Sistema | X = Ambos\n");
            Console.WriteLine($"Jogador: {posicaoUsuario} | Sistema: {posicaoSistema} | Linha de chegada: {limite}\n");
        }

        static string ExibirMenuContinuar()
        {
            Console.WriteLine("\n----------------------------------");
            Console.Write("Deseja jogar novamente?\n(S) Sim\n(N) Não\nOpção: ");
            string opcao = Console.ReadLine().ToUpper();
            while (opcao != "S" && opcao != "N")
            {
                Console.Write("Opção inválida. Digite S ou N: ");
                opcao = Console.ReadLine().ToUpper();
            }
            return opcao;
        }
    }
}

