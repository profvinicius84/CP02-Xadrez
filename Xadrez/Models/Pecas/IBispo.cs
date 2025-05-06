using Xadrez.Tabuleiro;
 
namespace Xadrez.Pecas
{
    public class Bispo : Peca
    {
        public Bispo(Cor cor, Tabuleiro tabuleiro) : base(cor, tabuleiro) { }
 
        public override bool[,] MovimentosPossiveis()
        {
            bool[,] matriz = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];
 
            Posicao pos = new Posicao(0, 0);
 
            // Direção: Noroeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            while (Tabuleiro.PosicaoValida(pos) && PodeMover(pos))
            {
                matriz[pos.Linha, pos.Coluna] = true;
                if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                    break;
                pos.DefinirValores(pos.Linha - 1, pos.Coluna - 1);
            }
 
            // Direção: Nordeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            while (Tabuleiro.PosicaoValida(pos) && PodeMover(pos))
            {
                matriz[pos.Linha, pos.Coluna] = true;
                if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                    break;
                pos.DefinirValores(pos.Linha - 1, pos.Coluna + 1);
            }
 
            // Direção: Sudeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            while (Tabuleiro.PosicaoValida(pos) && PodeMover(pos))
            {
                matriz[pos.Linha, pos.Coluna] = true;
                if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                    break;
                pos.DefinirValores(pos.Linha + 1, pos.Coluna + 1);
            }
 
            // Direção: Sudoeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            while (Tabuleiro.PosicaoValida(pos) && PodeMover(pos))
            {
                matriz[pos.Linha, pos.Coluna] = true;
                if (Tabuleiro.Peca(pos) != null && Tabuleiro.Peca(pos).Cor != Cor)
                    break;
                pos.DefinirValores(pos.Linha + 1, pos.Coluna - 1);
            }
 
            return matriz;
        }
 
        private bool PodeMover(Posicao pos)
        {
            Peca p = Tabuleiro.Peca(pos);
            return p == null || p.Cor != Cor;
        }
 
        public override string ToString()
        {
            return "B";
        }
    }
}