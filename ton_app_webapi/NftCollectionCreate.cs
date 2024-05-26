using TonSdk.Client;
using TonSdk.Contracts.nft;
using TonSdk.Contracts.Wallet;
using TonSdk.Core;
using TonSdk.Core.Block;
using TonSdk.Core.Crypto;

namespace ton_app_webapi
{
    public class NftCollectionCreate
    {

        public async Task<NftCollection?> CreateNftCollection(WalletV3 wallet, TonClient tonClient, Mnemonic mnemonic, uint? seqno)
        {
            // define nft collections options
            NftCollectionOptions opts = new NftCollectionOptions()
            {
                OwnerAddress = wallet.Address, // collection owner address
                RoyaltyAddress = wallet.Address, // address to receiving royalty payments
                CollectionContentUri = "https://davidishe.github.io/metadata/metadata.json", // collection metadata
                NftItemContentBaseUri = "https://davidishe.github.io/metadata", // nft content base
                Workchain = 0,
                Royalty = 0.2 // 10%, for example 0.05 - 5%, 0.3 - 30% etc
            };

            // create new NftCollection instance using options
            NftCollection collection = new NftCollection(opts);

            // creating collection deploy message
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
                            Body = null,
                            StateInit = collection.StateInit
                        }),
                        Mode = 3 // message mode
                    }
                }, seqno ?? 0).Sign(mnemonic.Keys.PrivateKey);

            // send this message via TonClient
            await tonClient.SendBoc(msg.Cell);

            // print collection contract address
            Console.WriteLine(collection.Address);
            return collection;




        }

    }
}

