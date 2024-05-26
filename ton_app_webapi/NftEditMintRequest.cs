using TonSdk.Client;
using TonSdk.Contracts.nft;
using TonSdk.Contracts.Wallet;
using TonSdk.Core;
using TonSdk.Core.Block;
using TonSdk.Core.Boc;
using TonSdk.Core.Crypto;

namespace ton_app_webapi
{
    public class NftEditMintRequest
    {


        public async Task NftEditMintRequestAsync(WalletV3 wallet, TonClient tonClient, Mnemonic mnemonic, NftCollection collection, uint? seqno)
        {
            // we will use same wallet and collection instance like in prev code block

            // create nft mint options
            var mintOptions = new NftEditContentOptions()
            {
                CollectionContentUri = "https://davidishe.github.io/metadata/metadata.json", // collection metadata
                NftItemContentBaseUri = "https://davidishe.github.io/", // nft content base
                RoyaltyAddress = wallet.Address, // address to receiving royalty payments
                Royalty = 0.6

                // ItemContentUri = "0.json" // nft content will be splitted with {baseUri}{contentUri} 

                // public string NftItemContentBaseUri { get; set; }
                // ItemIndex = 0, // item index to mint, if collection is empty then 0
                // Amount = new Coins(0.05), // amount send to nft item contract
                // ItemOwnerAddress = wallet.Address, // address which will own new nft
                // public Address RoyaltyAddress { get; set; }
                // public double Royalty { get; set; }
            };

            // create nft mint request body
            Cell nftMintBody = NftCollection.CreateEditContentRequest(mintOptions);


            // creating mint message
            var msg = wallet.CreateTransferMessage(new[]
            {
            new WalletTransfer
            {
                Message = new InternalMessage(new InternalMessageOptions
                {
                    Info = new IntMsgInfo(new IntMsgInfoOptions
                    {
                        Dest = collection.Address,
                        Value = new Coins(0.05)
                    }),
                    Body = nftMintBody
                }),
                Mode = 3 // message mode
            }
        }, seqno ?? 0).Sign(mnemonic.Keys.PrivateKey);

            // send this message via TonClient
            await tonClient.SendBoc(msg.Cell);

            // print item address
            Console.WriteLine(await tonClient.Nft.GetItemAddress(collection.Address, 0));

        }

    }
}