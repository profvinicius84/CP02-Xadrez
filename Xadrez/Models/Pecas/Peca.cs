using Xadrez.Models.Estrategias;

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
    public bool FoiMovimentada => MovimentosRelizados > 0;

    /// <summary>
    /// Quantidade de movimentos realizados pela peça.
    /// </summary>
    public int MovimentosRelizados { set; get; }

    /// <summary>
    /// Devolve o código da peça.
    /// </summary>
    public virtual string Codigo
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
                case Hades:
                    codigo = "H";
                    break;
            }
            return codigo;
        }
    }

    /// <summary>
    /// Número do último movimento realizado pela peça.
    /// </summary>
    public int UltimoMovimento { set; get; }

    /// <summary>
    /// Devolve a estratégia de movimento da peça.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <returns>Uma instância da classe EstrategiaMovimento que representa a estratégia de movimento da peça.</returns>
    public abstract IEstrategiaMovimento ObtemEstrategiaMovimento(ITabuleiro tabuleiro);

    /// <summary>
    /// Devolve lista de movimentos possíveis para a peça.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <returns>Uma lista de movimentos possíveis para a peça.</returns>
    public virtual List<Movimento> MovimentosPossiveis(ITabuleiro tabuleiro)
    {
        var estrategia = ObtemEstrategiaMovimento(tabuleiro);
        return estrategia.ObterMovimentosPossiveis();
    }

    public IPeca Clone()
    {        
        return (IPeca)MemberwiseClone();
    }
}