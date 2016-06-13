/*

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Kernel.Crypto;
using iText.Kernel.Pdf;

namespace iText.Kernel.Crypto.Securityhandler {
    public class PubSecHandlerUsingStandard40 : PubKeySecurityHandler {
        public PubSecHandlerUsingStandard40(PdfDictionary encryptionDictionary, X509Certificate[] certs, int[] permissions
            , bool encryptMetadata, bool embeddedFilesOnly) {
            InitKeyAndFillDictionary(encryptionDictionary, certs, permissions, encryptMetadata, embeddedFilesOnly);
        }

        public PubSecHandlerUsingStandard40(PdfDictionary encryptionDictionary, ICipherParameters certificateKey, 
            X509Certificate certificate, bool encryptMetadata) {
            InitKeyAndReadDictionary(encryptionDictionary, certificateKey, certificate, encryptMetadata);
        }

        public override OutputStreamEncryption GetEncryptionStream(Stream os) {
            return new OutputStreamStandardEncryption(os, nextObjectKey, 0, nextObjectKeySize);
        }

        public override IDecryptor GetDecryptor() {
            return new StandardDecryptor(nextObjectKey, 0, nextObjectKeySize);
        }

        protected internal override String GetDigestAlgorithm() {
            return "SHA-1";
        }

        protected internal override void InitKey(byte[] globalKey, int keyLength) {
            mkey = new byte[keyLength / 8];
            System.Array.Copy(globalKey, 0, mkey, 0, mkey.Length);
        }

        protected internal override void SetPubSecSpecificHandlerDicEntries(PdfDictionary encryptionDictionary, bool
             encryptMetadata, bool embeddedFilesOnly) {
            encryptionDictionary.Put(PdfName.Filter, PdfName.Adobe_PubSec);
            encryptionDictionary.Put(PdfName.R, new PdfNumber(2));
            PdfArray recipients = CreateRecipientsArray();
            encryptionDictionary.Put(PdfName.V, new PdfNumber(1));
            encryptionDictionary.Put(PdfName.SubFilter, PdfName.Adbe_pkcs7_s4);
            encryptionDictionary.Put(PdfName.Recipients, recipients);
        }
    }
}
