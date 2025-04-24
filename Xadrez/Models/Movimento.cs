using Xadrez.Models.Pecas;

namespace Xadrez.Models;

/// <summary>
/// Representa um movimento no jogo de xadrez.
/// </summary>
/// <param name="peca">Representa a peça que está sendo movida.</param>
/// <param name="casaOrigem">Representa a casa de origem da peça.</param>
/// <param name="casaDestino">Representa a casa de destino da peça.</param>
/// <param name="pecaCapturada">Representa a peça capturada, se houver.</param>
public class Movimento(IPeca peca, Casa casaOrigem, Casa casaDestino, IPeca? pecaCapturada = null)
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
}