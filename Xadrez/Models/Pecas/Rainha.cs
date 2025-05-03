namespace Xadrez.Models.Pecas
{
    public class Rainha(bool eBranca) : Peca(eBranca)
    {

        /// <summary>
        /// Devolve lista de movimentos possíveis para a peça.
        /// </summary>
        /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
        /// <returns>Uma lista de movimentos possíveis para a peça.</returns>
        public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
        {
            var movimentos = new List<Movimento>();

            foreach (var casa in tabuleiro.Casas)
            {
                if (casa.Peca is null || casa.Peca.EBranca != this.EBranca)
                {
                    movimentos.Add(new Movimento(this, tabuleiro.ObtemCasaPeca(this), casa, casa.Peca));
                }
            }

            return movimentos;
        }
    }
}
