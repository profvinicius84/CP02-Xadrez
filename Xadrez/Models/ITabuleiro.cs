using Xadrez.Models.Pecas;

namespace Xadrez.Models;

/// <summary>
/// Interface que representa um tabuleiro de xadrez.
/// </summary>
public interface ITabuleiro
{
    /// <summary>
    /// Cria e distribui as peças no tabuleiro.
    /// </summary>
    void DistribuiPecas();

    /// <summary>
    /// Valida se o movimento é válido.
    /// </summary>
    /// <param name="movimento"></param>
    /// <returns></returns>
    bool ValidaMovimento(Movimento movimento);
    bool ExecutaMovimento(Movimento movimento);
    bool ReverteMovimento(Movimento movimento);
    Casa ObtemCasaPeca(IPeca peca);
    bool VerificaXeque(bool eBranca);

    bool VerificaXequeMate(bool eBranca);
}