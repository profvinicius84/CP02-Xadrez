
namespace Xadrez.Models.Pecas
{
    public class Rainha(bool eBranca) : Peca(eBranca), IRainha
    {
        private int[][] movimentosRainha =
        {
            new[] {1, 0}, // baixo
            new[] {-1, 0}, // cima
            new[] {0, 1}, // direita
            new[] {0, -1}, // esquerda
            new[] {1, 1}, // diagonal direita baixo
            new[] {-1, -1}, // diagonal direita cima
            new[] {1, -1}, // diagonal esquerda baixo
            new[] {-1, 1}, // diagonal esquerda cima
        };
        public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
        {
            List<Movimento> movimentosPossiveis = new List<Movimento>();
            Casa casaOrigem = tabuleiro.ObtemCasaPeca(this);
            if(casaOrigem == null)
            {
                return movimentosPossiveis; // Rainha não está no tabuleiro

            }

            foreach (int[] movimento in movimentosRainha) {
                int linhaModificada = casaOrigem.Linha + movimento[0];
                int colunaModificada = casaOrigem.Coluna + movimento[1];
                while ((linhaModificada >= 0 && linhaModificada < 8) && (colunaModificada >= 0 && colunaModificada < 8)) {
                    Casa? casaEncontrada = tabuleiro.Casas.Find(casa => casa.Linha == linhaModificada && casa.Coluna == colunaModificada);
                    if(casaEncontrada != null)
                    {
                        if (casaEncontrada.Peca == null)
                        {
                            movimentosPossiveis.Add(new Movimento(this, casaOrigem, casaEncontrada));
                        }
                        else if (casaEncontrada.Peca.EBranca != this.EBranca)
                        {
                            movimentosPossiveis.Add(new Movimento(this, casaOrigem, casaEncontrada, casaEncontrada.Peca));
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    linhaModificada += movimento[0];
                    colunaModificada += movimento[1];
                }
            }
            return movimentosPossiveis;
        }
    }
}
