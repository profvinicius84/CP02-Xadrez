using Xadrez.Models.Pecas;

namespace Xadrez.Models.Pecas
{
    /// <summary>
    /// Representa a pe�a Torre no jogo de xadrez.
    /// </summary>
    /// <param name="eBranca">Define se a pe�a � branca (true) ou preta (false).</param>
    public class Torre(bool eBranca) : Peca(eBranca), ITorre
    {
        /// <summary>
        /// Retorna todos os movimentos v�lidos que a Torre pode fazer a partir da sua posi��o atual no tabuleiro.
        /// </summary>
        /// <param name="tabuleiro">O tabuleiro atual onde a torre est� posicionada.</param>
        /// <returns>Lista de movimentos poss�veis, incluindo movimentos de captura.</returns>
        public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
        {
            var movimentos = new List<Movimento>();

            // Pega a casa atual onde esta torre est� posicionada
            var casaAtual = tabuleiro.ObtemCasaPeca(this);

            if (casaAtual is null)
                return movimentos; // Se n�o tiver no tabuleiro n�o tem movimentos


            // for para calcular movimentos para cima

            for (int i = casaAtual.Linha + 1; i < 8; i++)
            {
                var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == i && c.Coluna == casaAtual.Coluna);
                if (casa == null) break;

                if (casa.Peca == null)
                {
                    movimentos.Add(new Movimento(this, casaAtual, casa));
                }
                else
                {
                    if (casa.Peca.EBranca != this.EBranca)
                        movimentos.Add(new Movimento(this, casaAtual, casa, casa.Peca));
                    break; // Para o movimento ao encontrar uma pe�a
                }
            }


            // for para calcular movimentos possiveis para baixo

            for (int i = casaAtual.Linha - 1; i >= 0; i--)
            {
                var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == i && c.Coluna == casaAtual.Coluna);
                if (casa == null) break;

                if (casa.Peca == null)
                {
                    movimentos.Add(new Movimento(this, casaAtual, casa));
                }
                else
                {
                    if (casa.Peca.EBranca != this.EBranca)
                        movimentos.Add(new Movimento(this, casaAtual, casa, casa.Peca));
                    break;
                }
            }


            // for para calcular movimentos possiveis para direita

            for (int j = casaAtual.Coluna + 1; j < 8; j++)
            {
                var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == casaAtual.Linha && c.Coluna == j);
                if (casa == null) break;

                if (casa.Peca == null)
                {
                    movimentos.Add(new Movimento(this, casaAtual, casa));
                }
                else
                {
                    if (casa.Peca.EBranca != this.EBranca)
                        movimentos.Add(new Movimento(this, casaAtual, casa, casa.Peca));
                    break;
                }
            }


            // for para calcular movimentos possiveis para esquerda

            for (int j = casaAtual.Coluna - 1; j >= 0; j--)
            {
                var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == casaAtual.Linha && c.Coluna == j);
                if (casa == null) break;

                if (casa.Peca == null)
                {
                    movimentos.Add(new Movimento(this, casaAtual, casa));
                }
                else
                {
                    if (casa.Peca.EBranca != this.EBranca)
                        movimentos.Add(new Movimento(this, casaAtual, casa, casa.Peca));
                    break;
                }
            }

            return movimentos;
        }
    }
}
