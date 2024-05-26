// See https://aka.ms/new-console-template for more information
using ton_app_webapi;
using TonSdk.Client;
using TonSdk.Contracts.Jetton;
using TonSdk.Contracts.nft;
using TonSdk.Contracts.Wallet;
using TonSdk.Core;
using TonSdk.Core.Block;
using TonSdk.Core.Boc;
using TonSdk.Core.Crypto;

Console.WriteLine("Hello, World!");


// create http parameters for ton client 
HttpParameters tonClientParams = new HttpParameters
{
  Endpoint = "https://testnet.toncenter.com/api/v2/jsonRPC",
  ApiKey = "38740ac622c70ed2bcdc27b2023854ef7b6fd818a18228f7d95406a815676afc"
};

// create ton client to fetch data and send boc
TonClient tonClient = new TonClient(TonClientType.HTTP_TONCENTERAPIV2, tonClientParams);

// create new mnemonic or use existing
string[] words = { "float", "virtual", "ranch", "bird", "include", "own", "outside", "obtain", "accuse", "worth", "helmet", "ask", "you", "legend", "spell", "improve", "wreck", "boost", "claim", "code", "guitar", "visit", "remember", "cloud" };



// byte[] seed = Mnemonic.GenerateSeed(words);

Mnemonic mnemonic = new Mnemonic(words);

// create wallet options
var optionsV3 = new WalletV3Options()
{
  PublicKey = mnemonic.Keys.PublicKey,
  Workchain = 0
};

// create wallet instance
WalletV3 wallet = new WalletV3(optionsV3, 2);

var nftCollectionCreate = new NftCollectionCreate();
var nftCreateTransferRequest = new NftCreateTransferRequest();
var nftCreateMintRequest = new NftCreateMintRequest();
var nftEditMintRequest = new NftEditMintRequest();



uint? seqno = await tonClient.Wallet.GetSeqno(wallet.Address);
var nftCollection = await nftCollectionCreate.CreateNftCollection(wallet, tonClient, mnemonic, seqno);


if (nftCollection is null)
{
  Console.WriteLine("Пустая NFT коллекция");
  return;
}

Console.WriteLine(nftCollection.Address.ToString());
var address = nftCollection.Address.ToString();
Console.WriteLine(address);
// await nftCreateTransferRequest.CreateTransferRequstNftAsync(wallet, tonClient, mnemonic, address, seqno);
// await nftEditMintRequest.NftEditMintRequestAsync(wallet, tonClient, mnemonic, nftCollection, seqno);






