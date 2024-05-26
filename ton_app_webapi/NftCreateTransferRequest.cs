using TonSdk.Client;
using TonSdk.Contracts.nft;
using TonSdk.Contracts.Wallet;
using TonSdk.Core;
using TonSdk.Core.Block;
using TonSdk.Core.Boc;
using TonSdk.Core.Crypto;

namespace ton_app_webapi
{
    public class NftCreateTransferRequest
    {


        // EQBFUaVDEhJwmAtxKVNHDs1ULqmN42rasKRWqeBSUZjBfz0I
        // EQBIm8YTfn_NHPO9DyOsDgXYwwNvXGnO9Kt8QP37h3Vn8AL7
        // kQA52XGkjSxf6qc9J8RS9BvA7uPdAw348ki_UDmDChMY9geI
        public async Task CreateTransferRequstNftAsync(WalletV3 wallet, TonClient tonClient, Mnemonic mnemonic, string address, uint? seqno)
        {

            // define the address of the nft collection
            Address collection = new Address(address: address);

            // define index of the nft what will send
            uint index = 11;

            // get the nft items address using TonClient: TonSdk.Client
            Address nftItemAddress = await tonClient.Nft.GetItemAddress(collection, index);

            // define receiver address or get receiver address from ton domain name system
            // Address receiver = new Address("/* destination address */");
            Address receiver = wallet.Address;

            // create transfer options
            NftTransferOptions options = new NftTransferOptions()
            {
                NewOwner = receiver
            };

            // create a message body for the nft transfer
            Cell nftTransfer = NftItem.CreateTransferRequest(options);

            // getting seqno using tonClient

            // create transfer message
            ExternalInMessage message = wallet.CreateTransferMessage(new[]
            {
                new WalletTransfer
                {
                    Message = new InternalMessage(new InternalMessageOptions
                    {
                        Info = new IntMsgInfo(new IntMsgInfoOptions
                        {
                            Dest = nftItemAddress,
                            Value = new Coins(0.1), // amount in TONs to send
                        }),
                        Body = nftTransfer
                    }),
                    Mode = 1 // message mode
                }
            }, seqno ?? 0); // if seqno is null we pass 0, wallet will auto deploy on message send

            // sign transfer message
            message.Sign(mnemonic.Keys.PrivateKey);

            // get message cell
            Cell cell = message.Cell;

            // send this message via TonClient
            await tonClient.SendBoc(cell);



        }


    }
}