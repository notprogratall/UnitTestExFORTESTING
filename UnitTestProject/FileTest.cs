using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using UnitTestEx;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    [TestClass]
    public class FileTest
    {

        public const string SIZE_EXCEPTION = "Wrong size";
        public const string NAME_EXCEPTION = "Wrong name";
        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public double length;


        /* Тестируем получение размера */
        [DataTestMethod]
        [DataRow(FILE_PATH_STRING, CONTENT_STRING)]
        [DataRow(SPACE_STRING, SPACE_STRING)]
        public void GetSizeTest(string filePath, string content)
        {
            File newFile = new File(filePath, content);
            length = content.Length / 2;
            Assert.AreEqual(length, newFile.GetSize(), SIZE_EXCEPTION);
        }

        /* Тестируем получение имени */
        [DataTestMethod]
        [DataRow(FILE_PATH_STRING, CONTENT_STRING)]
        [DataRow(SPACE_STRING, SPACE_STRING)]
        public void GetFilenameTest(string filePath, string content)
        {
            File newFile = new File(filePath, content);
            Assert.AreEqual(filePath, newFile.GetFilename(), NAME_EXCEPTION);
        }


    }
}
