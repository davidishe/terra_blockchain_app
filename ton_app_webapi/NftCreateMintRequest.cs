using TonSdk.Client;
using TonSdk.Contracts.nft;
using TonSdk.Contracts.Wallet;
using TonSdk.Core;
using TonSdk.Core.Block;
using TonSdk.Core.Boc;
using TonSdk.Core.Crypto;

namespace ton_app_webapi
{
    public class NftCreateMintRequest
    {


        public async Task MintNft(WalletV3 wallet, TonClient tonClient, Mnemonic mnemonic, NftCollection collection, uint? seqno)
        {
            // we will use same wallet and collection instance like in prev code block

            // create nft mint options
            NftMintOptions mintOptions = new NftMintOptions()
            {
                ItemIndex = 0, // item index to mint, if collection is empty then 0
                Amount = new Coins(0.05), // amount send to nft item contract
                ItemOwnerAddress = wallet.Address, // address which will own new nft
                ItemContentUri = "0.json" // nft content will be splitted with {baseUri}{contentUri} 
            };

            // create nft mint request body
            Cell nftMintBody = NftCollection.CreateMintRequest(mintOptions);


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