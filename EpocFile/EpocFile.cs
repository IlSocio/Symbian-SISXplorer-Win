using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using EpocData.MBM;
using EpocData.MIF;
using EpocData.RSC;
using EpocData.E32Image;
using System.Drawing;


namespace EpocData
{

    public enum UID1s
    {
        KNullUID = 0,
        KExecutableImageUid = 0x1000007A,
        KDynamicLibraryUid = 0x10000079,
        KDirectFileStoreLayoutUid = 0x10000037,
        KPermanentFileStoreLayoutUid = 0x10000050,
        ResourceFile = 0x101f4a6b,
    }


    public enum UID2s
    {
        KNullUID = 0,
        KSharedLibraryUid = 0x1000008d,     // Static Interface
        KUidApp = 0x100039CE,               // Polymorphic Interface
        KUidAppRegistrationResourceFile = 0x101f8021, 
        /* BIO - Forse sono UID3 di componenti gia' integrati
                KUidMsvBIODataStream = 0x10005268,
                KUidSmartMessageParserDll = 0x10001251,
                KUidBIOMessageTypeMtm = 0x10001262,
                KUidBIOMessageClientMtmDll = 0x1000125A,
                KUidBIOMessageServerMtmDll = 0x1000125C,
                KUidBIOMessageUiLayerMtmDll	= 0x10001265,
                KUidBIOMessageViewerApp = 0x10001264,*/

        // MTM
        ClientSideMTM = 0x10003C5F,
        ServerSideMTM = 0x10003C5E,
        UserInterfaceMTM = 0x10003C60,
        DataMTM = 0x10003C61,
        // TargetType
        KFileSystemUid = 0x100039df,
        FrontEndProcessor = 0x10005e32,
        Animation = 0x10003b22,
        EComPlugin = 0x10009D8D,
        NotifierForTextWindowServer = 0x101fe38b,
        PrinterDevice = 0x10003b1c,
        DataRecognizer = 0x10003a19,         // NON USATO IN 3rd EDITION
        // ALTRO
        KLogicalDeviceDriverUid = 0x100000af,
        KPhysicalDeviceDriverUid = 0x100039d0,
        KUidUnicodeCommServerModuleV02 = 0x10005054,
        KUidProtocolModule = 0x1000004A
    }


    public enum UID2s_TargetType
    {
        LDD = 0x100000af,
        PDD = 0x100039d0,
        CSY = 0x10005054,
        PRT = 0x1000004A,
        DLL = 0x1000008d,
        EXE = 0x100039CE,
        FEP = 0x10005e32,
        ANI = 0x10003b22,
        FSY = 0x100039df,
        PLUGIN = 0x10009D8D,
        TEXTNOTIFIER2 = 0x101fe38b,
        PDL = 0x10003b1c
    }


    public class EpocFile
    {
        public UInt32 uid1; // Epoc Header
        public UInt32 uid2; // Kind
        public UInt32 uid3; // Application ID

        public EpocFile(BinaryReader br)
        {
            uid1 = br.ReadUInt32();     // 37 00 00 10
            uid2 = br.ReadUInt32();     // 42 00 00 10
            uid3 = br.ReadUInt32();     // 00 00 00 00
        }


        public static EpocFile Factory(byte[] data)
        {
            if (data.Length < 4*4) 
                return null;

            BinaryReader br = new BinaryReader(new MemoryStream(data));
            UInt32 uid1 = 0;
            UInt32 uid2 = 0;
            try
            {
                uid1 = br.ReadUInt32();
                //Debug.Assert( uid1 == 0x10000037, "Not an EPOC File" );

                uid2 = br.ReadUInt32();
                br.BaseStream.Seek(-2 * sizeof(UInt32), SeekOrigin.Current);

                if (uid1 == (uint)UID1s.KDirectFileStoreLayoutUid && uid2 == 0x10000042)
                {
                    return new MBMArchive(br);
                }

                if (uid1 == 0x34232342 && uid2 == 0x00000002)
                {
                    return new MIFArchive(br);
                }

                if (uid1 == (uint) UID1s.ResourceFile)
                {
                    return new RSCFile(br);
                }

                return new E32File(br);
            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Exception:" + ex.ToString());
                return null;
            }
            finally
            {
                br.Close();
            }
        }
    }


    public abstract class IImage
    {
        public abstract byte[] GetData();

        public void SaveTo(string filename)
        {
            byte[] data = GetData();
            System.IO.File.WriteAllBytes(filename, data);
        }
    }


/*    public class ImagesCollectionEnumator : IEnumerator, ICollection
    {
        private List<IImage> list;
        private int pos;

        public ImagesCollectionEnumator(List<IImage> lista)
        {
            list = lista;
            pos = -1;
        }

        #region IEnumerator Members

        public object Current
        {
            get
            {
                if (pos < 0 || pos >= list.Count) return null;
                return list[pos];
            }
        }

        public bool MoveNext()
        {
            pos++;
            if (pos < 0 || pos >= list.Count)
                return false;
            return true;
        }

        public void Reset()
        {
            pos = -1;
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IsSynchronized
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public object SyncRoot
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }*/

}
