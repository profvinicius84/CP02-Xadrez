namespace Xadrez.Models.Pecas;

/// <summary>
/// Interface que representa um peão de xadrez.
/// </summary>
public interface IPeao : IPeca
{
    /// <summary>
    /// Verifica se o peão pode ser promovido. Um peão pode ser promovido quando chega na oitava linha (para as brancas) ou na primeira linha (para as pretas) do tabuleiro.
    /// </summary>
    /// <param name="tabuleiro"></param>
    /// <returns></returns>
    bool VarificaPromocao(Tabuleiro tabuleiro);

    /// <summary>
    /// Promove o peão para uma peça de maior valor (dama, torre, bispo ou cavalo).
    /// </summary>
    /// <param name="tabuleiro"></param>
    /// <param name="pecaPromocao"></param>
    void Promover(Tabuleiro tabuleiro, IPeca pecaPromocao);

    /// <summary>
    /// Peca que o peão se transforma quando é promovido.
    /// </summary>
    IPeca PecaPromocao { get; set; }
}
