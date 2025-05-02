namespace Xadrez.Models.Pecas
{
    public class Torre : ITorre
    {
        

       
        /// <summary>
        /// Indica se a peça é uma branca ou não.
        /// </summary>
        public Torre(bool eBranca)
        : base(eBranca)
        {
        }

        /// <summary>
        /// Indica se a peça foi movimentada ou não.
        /// </summary>
        public bool FoiMovimentada { set; get; }

        /// <summary>
        /// Devolve lista de movimentos possíveis para a peça.
        /// </summary>
        /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
        /// <returns>Uma lista de movimentos possíveis para a peça.</returns>
        public List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Devolve o código da peça.
        /// </summary>
        public string Codigo { get; }
    }
}
