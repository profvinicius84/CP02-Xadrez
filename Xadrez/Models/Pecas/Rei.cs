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
            if (RoqueExecutado)
                throw new InvalidOperationException("O roque já foi executado e não pode ser feito novamente.");

            if (tabuleiro.VerificaXeque(EBranca))
                return false;

            var casaRei = tabuleiro.ObtemCasaPeca(this);
            if (casaRei == null) return false;

            int linha = casaRei.Linha;
            int colunaRei = casaRei.Coluna;
            int colunaTorre = roquePequeno ? 7 : 0;

            var casaTorre = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == colunaTorre);
            if (casaTorre?.Peca is not ITorre torre || torre.EBranca != EBranca)
                return false;

            // Verificar se rei ou torre ainda estão em suas casas originais
            var casaReiInicial = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == 4); 
            var casaTorreInicial = casaTorre; 

            bool reiMoveu = casaReiInicial?.Peca != this;
            bool torreMoveu = casaTorreInicial?.Peca != torre;

            if (reiMoveu || torreMoveu)
                return false;

            int direcao = roquePequeno ? 1 : -1;
            int inicio = colunaRei + direcao;
            int fim = roquePequeno ? 6 : 1;

            for (int col = inicio; roquePequeno ? col <= fim : col >= fim; col += direcao)
            {
                var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == col);
                if (casa?.Peca != null)
                    return false;

                // Se tiver um método para verificar se o rei estaria em xeque ao passar por essa casa, use aqui
            }

            return true;
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
                    (0, 4), // roque pequeno (rei vai para coluna 6)
                    (0, -3) // roque grande (rei vai para coluna 2)
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

                        //if (casaDestino?.Peca == null || casaDestino.Peca.EBranca != EBranca)
                        //{
                        //if (!tabuleiro.VerificaXeque(true))
                        //{
                        movimentos.Add(new Movimento(this, casaRei, casaDestino));
                        //}
                        //}
                    }
                }
            //}

            return movimentos;
        }

        public class MovimentoDuplo : Movimento
        {
            public Movimento MovimentoTorre { get; }

            public MovimentoDuplo(Movimento movimentoRei, Movimento movimentoTorre)
                : base(movimentoRei.Peca, movimentoRei.CasaOrigem, movimentoRei.CasaDestino)
            {
                MovimentoTorre = movimentoTorre;
            }
        }
    }
}
