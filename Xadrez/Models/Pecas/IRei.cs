namespace Xadrez.Models.Pecas;

/// <summary>
/// Interface que define as propriedades e métodos específicos do Rei.
/// </summary>
public interface IRei
{
    /// <summary>
    /// Indica se o rei está em xeque.
    /// </summary>
    bool EmCheque { get; set; }

    /// <summary>
    /// Verifica se o roque é válido.
    /// </summary>
    /// <param name="tabuleiro">Tabuleiro onde o roque será realizado.</param>
    /// <param name="roquePequeno">Indica se é um roque pequeno (true) ou grande (false).</param>
    /// <returns>Retorna verdadeiro se o roque é válido, caso contrário, retorna falso.</returns>
    bool VerificaRoque(ITabuleiro tabuleiro, bool roquePequeno);

    /// <summary>
    /// Executa o roque.
    /// </summary>
    /// <param name="tabuleiro">Tabuleiro onde o roque será realizado.</param>
    /// <param name="roquePequeno">Indica se é um roque pequeno (true) ou grande (false).</param>
    void ExecutaRoque(ITabuleiro tabuleiro, bool roquePequeno);
}
