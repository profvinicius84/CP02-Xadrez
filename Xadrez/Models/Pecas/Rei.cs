namespace Xadrez.Models.Pecas
{
    public class Rei : IRei
    {


        /// <summary>
        /// Indica se a peça é uma branca ou não.
        /// </summary>
        public Rei(bool eBranca)
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
        /// Indica se o rei está em cheque ou não.
        /// </summary>
        bool EmCheque { get; set; }

        /// <summary>
        /// Verifica se o rei pode fazer o roque (pequeno ou grande).
        /// </summary>
        /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
        /// <param name="roquePequeno">Indica se o roque é pequeno ou grande. O roque pequeno é feito com a torre do lado do rei e o roque grande é feito com a torre do lado da dama.</param>
        /// <returns>Retorna verdadeiro se o roque é possível, caso contrário, retorna falso.</returns>
        public bool VerificaRoque(Tabuleiro tabuleiro, bool roquePequeno = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executa o roque (pequeno ou grande) no tabuleiro.
        /// </summary>
        /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
        /// <param name="roquePequeno">Indica se o roque é pequeno ou grande. O roque pequeno é feito com a torre do lado do rei e o roque grande é feito com a torre do lado da dama.</param>
        /// <returns>Retorna o movimento executado pelo roque. O movimento é composto pelo rei e pela torre que se movem ao mesmo tempo.</returns>
        public Movimento ExecutaRoque(Tabuleiro tabuleiro, bool roquePequeno = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Devolve o código da peça.
        /// </summary>
        public string Codigo { get; }
    }
}
