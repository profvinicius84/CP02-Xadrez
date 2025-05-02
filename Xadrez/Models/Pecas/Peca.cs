namespace Xadrez.Models.Pecas;

/// <summary>
/// Classe base para todas as peças do jogo de xadrez.
/// </summary>
/// <param name="eBranca">Indica se a peça é branca ou preta.</param>
public abstract class Peca(bool eBranca) : IPeca
{
    /// <summary>
    /// Indica se a peça é branca ou preta.
    /// </summary>
    public bool EBranca { get; } = eBranca;

    /// <summary>
    /// Indica se a peça foi movimentada ou não.
    /// </summary>
    public bool FoiMovimentada { get; set; }

    /// <summary>
    /// Devolve o código da peça.
    /// </summary>
    public string Codigo
    {
        get 
        {
            string codigo = "";
            switch (this)
            {
                case ITorre:
                    codigo = "T";
                    break;
                case ICavalo:
                    codigo = "C";
                    break;
                case IBispo:
                    codigo = "B";
                    break;
                case IRainha:
                    codigo = "D";
                    break;
                case IRei:
                    codigo = "R";
                    break;
                case IPeao:
                    codigo = "P";
                    break;
                case Hades:
                    codigo = "H";
                    break;
            }
            return codigo;
        }
    }

    /// <summary>
    /// Devolve lista de movimentos possíveis para a peça.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <returns>Uma lista de movimentos possíveis para a peça.</returns>
    public abstract List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro);
}