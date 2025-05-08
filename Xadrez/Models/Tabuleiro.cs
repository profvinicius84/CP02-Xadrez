using Xadrez.Models.Pecas;

namespace Xadrez.Models;


/// <summary>
/// Representa o tabuleiro de xadrez.
/// </summary>
public class Tabuleiro : ITabuleiro
{
    /// <summary>
    /// Representa o tamanho do tabuleiro de xadrez.
    /// </summary>
    public List<Casa> Casas { get; set; } = new();

    /// <summary>
    /// Pecas das partida
    /// </summary>
    public List<IPeca> Pecas { get; set; } = new();

    /// <summary>
    /// Pecas brancas da partida
    /// </summary>
    public List<IPeca> PecasBrancas => Pecas.Where(p => p.EBranca).ToList();

    /// <summary>
    /// Pecas pretas da partida
    /// </summary>
    public List<IPeca> PecasPretas => Pecas.Where(p => !p.EBranca).ToList();

    /// <summary>
    /// Representa a lista de peças capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasCapturadas { get; set; } = new();

    /// <summary>
    /// Representa a lista de peças brancas capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasBrancasCapturadas => PecasCapturadas.Where(p => p.EBranca).ToList();

    /// <summary>
    /// Representa a lista de peças pretas capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasPretasCapturadas => PecasCapturadas.Where(p => !p.EBranca).ToList();

    /// <summary>
    /// Construtor que inicializa o tabuleiro de xadrez com 64 casas.
    /// </summary>
    public Tabuleiro()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var casa = new Casa(i, j);
                Casas.Add(casa);
            }
        }
    }

    /// <summary>
    /// Distribui as peças no tabuleiro de xadrez. As peças brancas são colocadas nas linhas 1 e 2, enquanto as peças pretas são colocadas nas linhas 7 e 8.
    /// </summary>
    public void DistribuiPecas()
    {
        //Peças brancas
        Casas[0].Peca = new Torre(true);
        Casas[1].Peca = new Cavalo(true);
        Casas[2].Peca = new Bispo(true);
        Casas[3].Peca = new Rainha(true);
        Casas[4].Peca = new Rei(true);
        Casas[5].Peca = new Bispo(true);
        Casas[6].Peca = new Cavalo(true);
        Casas[7].Peca = new Torre(true);

        //for (int i = 8; i < 16; i++)
        //{
        //    Casas[i].Peca = new Peao(true);
        //}

        //Peças pretas
        Casas[63].Peca = new Torre(false);
        Casas[62].Peca = new Cavalo(false);
        Casas[61].Peca = new Bispo(false);
        Casas[60].Peca = new Rei(false);
        Casas[59].Peca = new Rainha(false);
        Casas[58].Peca = new Bispo(false);
        Casas[57].Peca = new Cavalo(false);
        Casas[56].Peca = new Torre(false);

        //for (int i = 48; i < 56; i++)
        //{
        //    Casas[i].Peca = new Peao(false);
        //}

        Pecas.AddRange(Casas.Where(c => c.Peca is not null).Select(c => c.Peca));
    }

    public bool ValidaMovimento(Jogador jogador, Movimento movimento)
    {
        if (jogador is not null && jogador.EBranco != movimento.Peca.EBranca)
            return false;
        if (PecasCapturadas.Contains(movimento.Peca) || PecasCapturadas.Contains(movimento.PecaCapturada))
            return false;
        if (!Casas.Contains(movimento.CasaOrigem) || !Casas.Contains(movimento.CasaDestino))
            return false;
        if (movimento.Peca != movimento.CasaOrigem.Peca)
            return false;
        if (movimento.PecaCapturada is not null)
        {
            if (movimento.PecaCapturada.EBranca == movimento.Peca.EBranca)
                return false;            
            if (movimento.Peca is IRei && movimento.PecaCapturada is IRei)
                return false;
        }
        if (movimento.ERoque)
        {
            if (movimento.Peca is not IRei)
                return false;
            if (!(movimento.Peca as IRei).VerificaRoque(this) && !(movimento.Peca as IRei).VerificaRoque(this, true))
                return false;
        }

        return true;
    }


    /// <summary>
    /// Executa um movimento no tabuleiro.
    /// </summary>
    /// <param name="movimento">Movimento a ser executado.</param>
    public void ExecutaMovimento(Movimento movimento)
    {
        if (movimento.PecaCapturada is not null)
        {
            movimento.CasaDestino.Peca = null;
            PecasCapturadas.Add(movimento.PecaCapturada);
        }
        movimento.CasaDestino.Peca = movimento.Peca;
        movimento.CasaOrigem.Peca = null;
        if (movimento.ERoque)
        {
            //if (movimento.CasaDestino.Coluna == 6) //Roque pequeno
            //{
            //    var casaTorre = ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 7);
            //    ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 5).Peca = casaTorre.Peca;
            //    casaTorre.Peca = null;
            //}
            //else //Roque grande
            //{
            //    var casaTorre = ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 0);
            //    ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 3).Peca = casaTorre.Peca;
            //    casaTorre.Peca = null;
            //}
        }
    }


    /// <summary>
    /// Reverte um movimento no tabuleiro (desfaz o último movimento).
    /// </summary>
    /// <param name="movimento">Movimento a ser revertido.</param>
    public void ReverteMovimento(Movimento movimento)
    {
        movimento.CasaOrigem.Peca = movimento.Peca;
        movimento.CasaDestino.Peca = null;
        if (movimento.PecaCapturada is not null)
        {
            movimento.CasaDestino.Peca = movimento.PecaCapturada;
            PecasCapturadas.Remove(movimento.PecaCapturada);
        }
        if (movimento.ERoque)
        {
            //if (movimento.CasaDestino.Coluna == 6) //Roque pequeno
            //{
            //    var casaTorre = ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 5);
            //    ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 7).Peca = casaTorre.Peca;
            //    casaTorre.Peca = null;
            //}
            //else //Roque grande
            //{
            //    var casaTorre = ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 3);
            //    ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 0).Peca = casaTorre.Peca;
            //    casaTorre.Peca = null;
            //}
        }        
    }


    /// <summary>
    /// Obtém a casa atual de uma peça no tabuleiro.
    /// </summary>
    /// <param name="peca">Peça a ser localizada.</param>
    /// <returns>A casa onde a peça se encontra, ou null se não encontrada.</returns>
    public Casa? ObtemCasaPeca(IPeca peca)
    {
        return Casas.FirstOrDefault(c => c.Peca == peca || (c.Peca is IPeao && (c.Peca as IPeao).PecaPromocao == peca));
    }


    //Grupo 8 - Feito
    public bool VerificaXeque(bool eBranca)
    {
        // Seleciona as peças do jogador conforme a cor informada
        var pecasDoJogador = eBranca ? PecasBrancas : PecasPretas;

        // Encontra o rei do jogador
        IPeca? rei = pecasDoJogador.FirstOrDefault(p => p is IRei);
        if (rei == null)
        {
            // Não encontrou o rei — talvez o jogo esteja em um estado inválido
            return false;
        }

        // Encontra a casa onde está o rei
        Casa? casaDoRei = ObtemCasaPeca(rei);
        if (casaDoRei == null)
        {
            // O rei não está posicionado em nenhuma casa — também um estado inválido
            return false;
        }

        // Seleciona as peças inimigas
        var pecasInimigas = eBranca ? PecasPretas : PecasBrancas;

        // Verifica se alguma peça inimiga pode capturar o rei
        foreach (var pecaInimiga in pecasInimigas)
        {
            var movimentosInimigos = pecaInimiga.MovimentosPossiveis(this);
            foreach (var movimento in movimentosInimigos)
            {
                if (movimento.CasaDestino == casaDoRei)
                {
                    // O rei está sob ameaça
                    return true;
                }
            }
        }

        // Nenhuma peça inimiga ameaça o rei
        return false;
    }

    public bool VerificaXequeMate(bool eBranca)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Verifica se uma casa está sob ataque.
    /// </summary>
    /// <param name="casa">Casa a ser verificada.</param>
    /// <param name="eBranca">Indica se o jogador é branco ou preto.</param>
    /// <returns>Retorna verdadeiro se a casa está sob ataque, caso contrário, falso.</returns>
    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        //foreach (var linha in Casas)
        //{
        //    foreach (var outraCasa in linha)
        //    {
        //        if (outraCasa.Peca != null && outraCasa.Peca.EBranca != eBranca)
        //        {
        //            var movimentosPossiveis = outraCasa.Peca.MovimentosPossiveis(this);
        //            foreach (var movimento in movimentosPossiveis)
        //            {
        //                if (movimento.Destino == casa)
        //                    return true;
        //            }
        //        }
        //    }
        //}
        return false;
    }
}