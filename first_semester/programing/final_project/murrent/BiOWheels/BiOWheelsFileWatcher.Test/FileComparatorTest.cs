// *******************************************************
// * <copyright file="FileComparatorTest.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    /// <summary>
    /// </summary>
    public class FileComparatorTest
    {
        /// <summary>
        /// </summary>
        private const string FileNameSourceFile = "A.txt";

        /// <summary>
        /// </summary>
        private const string FileNameDestinationFile = "B.txt";

        /// <summary>
        /// </summary>
        private const string FileNameSourceFileSame = "AI.txt";

        /// <summary>
        /// </summary>
        private const string FileNameDestinationFileSame = "BII.txt";

        /// <summary>
        /// List holding random text snippets
        /// </summary>
        private readonly List<string> randomText = new List<string>();

        /// <summary>
        /// Represents the <see cref="FileComparator"/> instance
        /// </summary>
        private FileComparator fileComparator;

        /// <summary>
        /// Set up test environment
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.fileComparator = new FileComparator { BlockSize = 200 };

            this.randomText.Add(
                "Literature admiration frequently indulgence announcing are who you her. Was least quick after six. So it yourself repeated together cheerful. Neither it cordial so painful picture studied if. Sex him position doubtful resolved boy expenses. Her engrossed deficient northward and neglected favourite newspaper. But use peculiar produced concerns ten. In friendship diminution instrument so. Son sure paid door with say them. Two among sir sorry men court. Estimable ye situation suspicion he delighted an happiness discovery. Fact are size cold why had part. If believing or sweetness otherwise in we forfeited. Tolerably an unwilling arranging of determine. Beyond rather sooner so if up wishes or. Preserved defective offending he daughters on or. Rejoiced prospect yet material servants out answered men admitted. Sportsmen certainty prevailed suspected am as. Add stairs admire all answer the nearer yet length. Advantages prosperous remarkably my inhabiting so reasonably be if. Too any appearance announcing impossible one. Out mrs means heart ham tears shall power every. Be me shall purse my ought times. Joy years doors all would again rooms these. Solicitude announcing as to sufficient my. No my reached suppose proceed pressed perhaps he. Eagerness it delighted pronounce repulsive furniture no. Excuse few the remain highly feebly add people manner say. It high at my mind by roof. No wonder worthy in dinner. ");
            this.randomText.Add(
                "To they four in love. Settling you has separate supplied bed. Concluded resembled suspected his resources curiosity joy. Led all cottage met enabled attempt through talking delight. Dare he feet my tell busy. Considered imprudence of he friendship boisterous. ");
            this.randomText.Add(
                "Do commanded an shameless we disposing do. Indulgence ten remarkably nor are impression out. Power is lived means oh every in we quiet. Remainder provision an in intention. Saw supported too joy promotion engrossed propriety. Me till like it sure no sons.");
        }

        /// <summary>
        /// Tests the file compare method with file with different content 
        /// </summary>
        [TestCase]
        public void TestFileComparatorWithDifferentFiles()
        {
            this.CreateFilesWithDifferentContentIfNotExist();

            this.fileComparator.Compare(FileNameSourceFile, FileNameDestinationFile);

            Assert.True(this.FileEquals(FileNameSourceFile, FileNameDestinationFile));
        }

        /// <summary>
        /// Tests the file compare method with file with the same content
        /// </summary>
        [TestCase]
        public void TestFileComparatorWithSameFiles()
        {
            this.CreateFilesWithSameContentIfNotExist();

            this.fileComparator.Compare(FileNameSourceFileSame, FileNameDestinationFileSame);

            Assert.True(this.FileEquals(FileNameSourceFileSame, FileNameDestinationFileSame));
        }

        /// <summary>
        /// Creates two files with a different content
        /// </summary>
        private void CreateFilesWithDifferentContentIfNotExist()
        {
            using (StreamWriter streamWriter = new StreamWriter(FileNameSourceFile, false))
            {
                streamWriter.Write(this.randomText[0]);
            }

            using (StreamWriter streamWriter = new StreamWriter(FileNameDestinationFile, false))
            {
                streamWriter.Write("BACD");
            }
        }

        /// <summary>
        /// Creates two files with the same content
        /// </summary>
        private void CreateFilesWithSameContentIfNotExist()
        {
            using (StreamWriter streamWriter = new StreamWriter(FileNameSourceFileSame, false))
            {
                streamWriter.Write(this.randomText[2]);
            }

            using (StreamWriter streamWriter = new StreamWriter(FileNameDestinationFileSame, false))
            {
                streamWriter.Write(this.randomText[2]);
            }
        }

        /// <summary>
        /// Checks if files are equal
        /// </summary>
        /// <param name="path1">First file</param>
        /// <param name="path2">Second file</param>
        /// <returns>A boolean value indicating whether the files are equal or not</returns>
        private bool FileEquals(string path1, string path2)
        {
            byte[] file1 = File.ReadAllBytes(path1);
            byte[] file2 = File.ReadAllBytes(path2);

            if (file1.Length == file2.Length)
            {
                return !file1.Where((t, i) => t != file2[i]).Any();
            }

            return false;
        }
    }
}