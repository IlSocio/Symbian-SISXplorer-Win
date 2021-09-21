using System;
using System.Collections.Generic;
using System.Text;

namespace MBM
{
    class Rle
    {
    }
}

/*

						int nWidth = m_HeaderInfo.nWidth;
						int nHeight = m_HeaderInfo.nHeight;
						int bytesPerPixelPerChannel = m_HeaderInfo.nBitsPerPixel/8;
						
						int nPixels = nWidth * nHeight;
						int nTotalBytes = nPixels * bytesPerPixelPerChannel * m_HeaderInfo.nChannels;

						byte [] pDest = new byte[nTotalBytes];
						byte [] pData = new byte[nTotalBytes];
						for(long i=0;i<nTotalBytes;i++) pData[i] = 254;
						for(long i=0;i<nTotalBytes;i++) pDest[i] = 254;

						byte ByteValue = 0x00;

						int Count = 0;

						int nPointer = 0;

						// The RLE-compressed data is proceeded by a 2-byte data count for each row in the data,
						// which we're going to just skip.
						stream.Position += nHeight * m_HeaderInfo.nChannels*2;


						for(int channel=0; channel<m_HeaderInfo.nChannels; channel++)
						{
							// Read the RLE data.
							Count = 0;
							while(Count<nPixels)
							{
								ByteValue = binReader.ReadByte();

								int len = (int)ByteValue;
								if(len < 128)
								{
									len++;
									Count += len;

									while(len!=0)
									{
										ByteValue = binReader.ReadByte();

										pData[nPointer] = ByteValue;
										nPointer++;
										len--;
									}
								}
								else if(len > 128)
								{
									// Next -len+1 bytes in the dest are replicated from next source byte.
									// (Interpret len as a negative 8-bit int.)
									len ^= 0x0FF;
									len += 2;
									ByteValue = binReader.ReadByte();

									Count += len;

									while(len!=0)
									{
										pData[nPointer] = ByteValue;
										nPointer++;
										len--;
									}
								}
								else if ( 128 == len )
								{
									// Do nothing
								}
							}
						}

						int nPixelCounter = 0;
						nPointer = 0;

						for(int nColour = 0; nColour<m_HeaderInfo.nChannels; ++nColour)
						{
							nPixelCounter = nColour*bytesPerPixelPerChannel;
							for(int nPos=0; nPos<nPixels; ++nPos)
							{
								for(int j=0;j<bytesPerPixelPerChannel;j++)
									pDest[nPixelCounter+j] = pData[nPointer+j];

								nPointer++;

								nPixelCounter += m_HeaderInfo.nChannels*bytesPerPixelPerChannel;
							}
						}

						for(int i=0;i<nTotalBytes;i++) pData[i]=pDest[i];
						
						int ppm_x = 3780;	// 96 dpi
						int ppm_y = 3780;	// 96 dpi

						if(m_bResolutionInfoFilled)
						{
							int nHorResolution = (int)m_ResolutionInfo.hRes;
							int nVertResolution = (int)m_ResolutionInfo.vRes;

							ppm_x = (nHorResolution * 10000 )/254;
							ppm_y = (nVertResolution * 10000 )/254;
						}

						switch (m_HeaderInfo.nBitsPerPixel)
						{
							case 1:
							{
								nErrorCode = -7; // Not yet implemented
							}
								break;
							case 8:
							case 16:
							{
								CreateDIBSection(nWidth, nHeight, ppm_x, ppm_y, 24);
							}
								break;
							default:
							{
								nErrorCode = -8;	// Unsupported format
							}
								break;
						}

						IntPtr hBitmap = m_hBitmap;

						if(hBitmap == IntPtr.Zero)
						{
							nErrorCode = -9;	// Cannot create hBitmap
							return nErrorCode;
						}

						ProccessBuffer(pData);
					}

*/