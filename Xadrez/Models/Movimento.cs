using Xadrez.Models.Pecas;

namespace Xadrez.Models;

/// <summary>
/// Representa um movimento no jogo de xadrez.
/// </summary>
/// <param name="peca">Representa a peça que está sendo movida.</param>
/// <param name="casaOrigem">Representa a casa de origem da peça.</param>
/// <param name="casaDestino">Representa a casa de destino da peça.</param>
/// <param name="pecaCapturada">Representa a peça capturada, se houver.</param>
public class Movimento(IPeca peca, Casa casaOrigem, Casa casaDestino, IPeca? pecaCapturada = null, bool eRoque = false, bool ECheque = false, bool EChequeMate = false)
{    
    /// <summary>
    /// Representa a peça que está sendo movida.
    /// </summary>
    public IPeca Peca { get;} = peca;

    /// <summary>
    /// Representa a casa de origem da peça.
    /// </summary>
    public Casa CasaOrigem { get;} = casaOrigem;

    /// <summary>
    /// Representa a casa de destino da peça.
    /// </summary>
    public Casa CasaDestino { get; } = casaDestino;

    /// <summary>
    /// Representa a peça capturada, se houver.
    /// </summary>
    public IPeca? PecaCapturada { get;} = pecaCapturada;


    /// <summary>
    /// Indica se o movimento é um roque ou não.
    /// </summary>
    public bool ERoque { get; } = eRoque;

    /// <summary>
    /// Verifica se o rei esta em cheque 
    /// </summary>
    public bool ECheque { get; } = eCheque;

    /// <summary>
    /// Verifica se o rei esta em cheque mate
    /// </summary>
    public bool EChequeMate { get; } = eChequeMate;


    /// <summary>
    /// Representa a notação algébrica do movimento.
    /// </summary>
    public string NotacaoAlgebrica => $"{Peca.Codigo}{CasaOrigem.Codigo.ToLower()}{(PecaCapturada is not null ? "x" : "")}{CasaDestino.Codigo.ToLower()}";
}