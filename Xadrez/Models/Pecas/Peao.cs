namespace Xadrez.Models.Pecas
{
    public class Peao(bool eBranca) : IPeao
    {
        public bool VarificaPromocao(Tabuleiro tabuleiro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Promove o peão para uma peça de maior valor (dama, torre, bispo ou cavalo).
        /// </summary>
        /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
        /// <param name="pecaPromocao">A peça que o peão se transforma quando é promovido.</param>
        public void Promover(Tabuleiro tabuleiro, IPeca pecaPromocao){
            throw new NotImplementedException();

        }

        /// <summary>
        /// Peca que o peão se transforma quando é promovido.
        /// </summary>
        public IPeca PecaPromocao { get; set; }

        public bool EBranca { get; set; }
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
