using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace SISX.Fields
{
    public abstract class SISField
    {
        public readonly UInt64 length;


        protected SISField(BinaryReader br)
        {
            UInt32 len32 = br.ReadUInt32();
            length = len32;
            if ((len32 & 0x80000000) != 0) // Verifica il bit + significativo
            {
                len32 = len32 & 0x7FFFFFFF;
                length = len32 << 32;
                len32 = br.ReadUInt32();
                length = length | len32;
            }
            long oldPos = br.BaseStream.Position;
            ReadValue(br);

            // Legge i restanti bytes di padding eventualmente presenti per allineare il SISField a 32bit
            long qtaRead = br.BaseStream.Position - oldPos;
            while (qtaRead % 4 != 0)
            {
                int padByte = br.ReadByte();
                System.Diagnostics.Debug.Assert(padByte == 0);
                qtaRead++;
            }
        }

        protected abstract void ReadValue(BinaryReader br);

        /// <summary>
        /// Metodo utilizzato per la creazione degli elementi che costituiscono un SISArray
        /// </summary>
        public static SISField Factory(BinaryReader br, UInt32 id)
        {
            switch (id)
            {
                case 2:
                    return new SISArray(br);
                case 3:
                    return new SISCompressed(br);
                case 12:
                    return new SISContent(br);
                case 30:
                    return new SISData(br);
                case 31:
                    return new SISDataUnit(br);
                case 32:
                    return new SISFileData(br);
                case 34:
                    return new SISControllerChecksum(br);
                case 35:
                    return new SISDataChecksum(br);

                // Fields che entrano in gioco dopo che e' stato effettuato l'unzip del controller
                case 11:
                    return new SISLanguage(br);
                case 13:
                    return new SISController(br);
                case 14:
                    return new SISInfo(br);
                case 16:
                    return new SISSupportedOptions(br);
                case 33:
                    return new SISSupportedOption(br);
                case 15:
                    return new SISSupportedLanguages(br);
                case 17:
                    return new SISPrerequisites(br);
                case 19:
                    return new SISProperties(br);
                case 23:
                    return new SISLogo(br);
                case 28:
                    return new SISInstallBlock(br);
                case 39:
                    return new SISSignatureCertificateChain(br);
                case 40:
                    return new SISDataIndex(br);
                case 9:
                    return new SISUid(br);
                case 1:
                    return new SISString(br);
                case 4:
                    return new SISVersion(br);
                case 8:
                    return new SISDateTime(br);
                case 6:
                    return new SISDate(br);
                case 7:
                    return new SISTime(br);
                case 5:
                    return new SISVersionRange(br);
                case 18:
                    return new SISDependency(br);
                case 20:
                    return new SISProperty(br);
                case 24:
                    return new SISFileDescription(br);
                case 41:
                    return new SISCapabilities(br);
                case 25:
                    return new SISHash(br);
                case 37:
                    return new SISBlob(br);
                case 36:
                    return new SISSignature(br);
                case 38:
                    return new SISSignatureAlgorithm(br);
                case 22:
                    return new SISCertificateChain(br);
//                case 21:
                //                    return new SISSignatures( br );  // e' un SISArray di SISSignature
                case 26:
                    return new SISIf( br );
                case 27:
                    return new SISElseIf( br );
                case 29:
                    return new SISExpression( br );

                default:
                    // 0: Invalid
                    // 10: Unused
                    throw new Exception("SISField Not Found" + id);
            }
        }


        public static SISField Factory(BinaryReader br)
        {
            UInt32 id = br.ReadUInt32();
            SISField field = Factory(br, id);
            return field;
        }

    }
}
