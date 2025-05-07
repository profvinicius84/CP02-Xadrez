using System;
using System.Linq;

namespace Xadrez.Models.Pecas
{
    public class Rei(bool eBranca) : Peca(eBranca), IRei
    {
        public bool EmCheque { get; set; }

        public bool RoqueExecutado { get; private set; }
        public bool FoiMovimentada { get; set; }

        //public Rei(bool eBranca) : base(eBranca)
        //{
        //    EmCheque = false;
        //    RoqueExecutado = false;
        //    FoiMovimentada = false;
        //}

        public bool VerificaRoque(Tabuleiro tabuleiro, bool roquePequeno = false)
        {
            var casaRei = tabuleiro.ObtemCasaPeca(this);
            if (casaRei == null)
                throw new InvalidOperationException("Rei não está no tabuleiro.");

            // Verifica se o rei já se moveu
            if (FoiMovimentada)
                return false;

            // Encontre a torre correspondente
            Torre torre = null;
            int colunaTorre = roquePequeno ? 7 : 0; // Coluna da torre para roque pequeno ou grande
            var casaTorre = tabuleiro.Casas.FirstOrDefault(c => c.Linha == casaRei.Linha && c.Coluna == colunaTorre);

            if (casaTorre?.Peca is not Torre)
                return false; 

            // Verifica se a torre já se moveu
            if (torre == null || torre.FoiMovimentada)
                return false;

            // Verifica se as casas entre o rei e a torre estão desocupadas
            int passo = roquePequeno ? 1 : -1; // Direção do roque
            int colunaInicio = roquePequeno ? 4 : 5; // Coluna inicial para verificar as casas entre rei e torre
            int colunaFinal = roquePequeno ? 6 : 2; // Coluna final onde o rei irá

            for (int coluna = colunaInicio; coluna != colunaFinal; coluna += passo)
            {
                var casaIntermediaria = tabuleiro.Casas.FirstOrDefault(c => c.Linha == casaRei.Linha && c.Coluna == coluna);
                if (casaIntermediaria?.Peca != null)
                    return false; // Alguma casa entre o rei e a torre está ocupada
            }

            return true; // Todas as condições para o roque foram atendidas
        }


        public Movimento ExecutaRoque(Tabuleiro tabuleiro, bool roquePequeno = false)
        {
            var casaRei = tabuleiro.ObtemCasaPeca(this);
            if (casaRei == null)
                throw new InvalidOperationException("Rei não está no tabuleiro.");

            int linha = casaRei.Linha;
            int colunaRei = casaRei.Coluna;
            int colunaTorre = roquePequeno ? 7 : 0;
            var casaTorre = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == colunaTorre);
            if (casaTorre?.Peca is not ITorre torre)
                throw new InvalidOperationException("Torre não encontrada para o roque.");

            // Verificar se o rei ou a torre ainda estão nas casas originais
            var casaReiInicial = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == 3); // 4 é a coluna inicial do rei
            var casaTorreInicial = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == (roquePequeno ? 7 : 0));

            bool reiMoveu = casaReiInicial?.Peca != this;
            bool torreMoveu = casaTorreInicial?.Peca != torre;

            if (reiMoveu || torreMoveu)
                throw new InvalidOperationException("O rei ou a torre já se moveram, não é possível realizar o roque.");

            // Destinos do roque
            var destinoRei = tabuleiro.Casas.First(c => c.Linha == linha && c.Coluna == (roquePequeno ? 6 : 2));
            var destinoTorre = tabuleiro.Casas.First(c => c.Linha == linha && c.Coluna == (roquePequeno ? 5 : 3));

            // Atualiza as casas
            casaRei.Peca = null;
            destinoRei.Peca = this;

            casaTorre.Peca = null;
            destinoTorre.Peca = torre;

            // Marcar que o roque foi executado
            RoqueExecutado = true;

            // Retorna o movimento realizado (movimento do rei)
            return new Movimento(this, casaRei, destinoRei);
        }

        public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
        {
            var movimentos = new List<Movimento>();
            var casaRei = tabuleiro.ObtemCasaPeca(this);

            if (casaRei == null) return movimentos;

            //foreach (var casa in tabuleiro.Casas)
            //{
            int linha = casaRei.Linha;
            int coluna = casaRei.Coluna;

            var direcoes = new (int linha, int coluna)[]
            {
            (-1, 0),
            (1, 0),
            (0, -1),
            (0, 1),
            (-1, -1),
            (-1, 1),
            (1, -1),
            (1, 1),
            };

            // Adcionado para atender a direção do roque
            if ((linha == 0 || linha == 7) && coluna == 3)
            {

                var direcoesExtras = new (int, int)[]
                {
                    (0, 3), // roque pequeno (rei vai para coluna 6)
                    (0, -2) // roque grande (rei vai para coluna 2)
                };

                Array.Resize(ref direcoes, direcoes.Length + direcoesExtras.Length);
                for (int i = 0; i < direcoesExtras.Length; i++)
                {
                    direcoes[^(direcoesExtras.Length - i)] = direcoesExtras[i];
                }
            }

            foreach (var (dLinha, dColuna) in direcoes)
            {
                int novaLinha = linha + dLinha;
                int novaColuna = coluna + dColuna;

                if (novaLinha >= 0 && novaLinha < 8 && novaColuna >= 0 && novaColuna < 8)
                {
                    var casaDestino = tabuleiro.Casas.FirstOrDefault(c => c.Linha == novaLinha && c.Coluna == novaColuna);

                    if (casaDestino?.Peca == null || casaDestino.Peca.EBranca != EBranca)
                    {
                        //if (!tabuleiro.VerificaXeque(true))
                        //{
                        //movimentos.Add(new Movimento(this, casaRei, casaDestino));
                        //}

                        if (IsRoque(casaDestino))
                        {
                            //if (VerificaRoque(tabuleiro, false))
                            //{
                            var movimentoRoque = new Movimento(this, casaRei, casaDestino, null, true);

                            var bloqueado = false;

                            if ((casaDestino.Linha == 0 || casaDestino.Linha == 7) && casaDestino.Coluna == 1) // Roque Pequeno
                            { 
                                var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == 0 && c.Coluna == 2);

                                if (casa?.Peca != null)
                                {
                                    bloqueado = true;
                                }

                                if (!bloqueado && casaDestino?.Peca == null)
                                {
                                    movimentos.Add(movimentoRoque);
                                }
                            }
                            else if ((casaDestino.Linha == 0 || casaDestino.Linha == 7) && casaDestino.Coluna == 6) // Roque Grande
                            {
                                var l = 0;
                                for (int col = 4; col < 6; col++)
                                {
                                    var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == l && c.Coluna == col);
                                    if (casa?.Peca != null)
                                    {
                                        bloqueado = true;
                                        break;
                                    }
                                }

                                if (!bloqueado && casaDestino?.Peca == null)
                                {
                                    movimentos.Add(movimentoRoque);
                                }
                            }
                            else
                            {
                                movimentos.Add(new Movimento(this, casaRei, casaDestino));
                            }
                            //}
                        } else {
                            //if (!tabuleiro.VerificaXeque(true))
                            //{
                            movimentos.Add(new Movimento(this, casaRei, casaDestino));
                            //}
                        }
                    }
                }
            }
            //}

            return movimentos;
        }

        public bool IsRoque(Casa casaDestino)
        {
            if (casaDestino.Linha == 0 || casaDestino.Linha == 7) // Peça Branca ou Preta
            {
                if (casaDestino.Coluna == 1) // Roque Pequeno
                {
                    return true;
                } 
                else if (casaDestino.Coluna == 6) // Roque Grande
                {
                    return true;
                }
            }

            return false;
        }
    }
}
