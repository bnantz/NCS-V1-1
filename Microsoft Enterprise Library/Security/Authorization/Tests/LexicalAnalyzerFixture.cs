//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System;
using System.Collections;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Authorization.Tests
{
    [TestFixture]
    public class LexicalAnalyzerFixture
    {
        [Test]
        public void AndTokenTest()
        {
            LexicalAnalyzer lexer = new LexicalAnalyzer("and");
            TokenType token = lexer.MoveNext();
            Assert.AreEqual(TokenType.And, token);
            Assert.AreEqual("and", lexer.Current);
        }

        [Test]
        public void OrTokenTest()
        {
            LexicalAnalyzer lexer = new LexicalAnalyzer("or");
            TokenType token = lexer.MoveNext();
            Assert.AreEqual(TokenType.Or, token);
            Assert.AreEqual("or", lexer.Current);
        }

        [Test]
        public void NotTokenTest()
        {
            LexicalAnalyzer lexer = new LexicalAnalyzer("not");
            TokenType token = lexer.MoveNext();
            Assert.AreEqual(TokenType.Not, token);
            Assert.AreEqual("not", lexer.Current);
        }

        [Test]
        public void RightParenthesisTest()
        {
            LexicalAnalyzer lexer = new LexicalAnalyzer(")");
            TokenType token = lexer.MoveNext();
            Assert.AreEqual(TokenType.RightParenthesis, token);
            Assert.AreEqual(")", lexer.Current);
        }

        [Test]
        public void LeftParenthesisTest()
        {
            LexicalAnalyzer lexer = new LexicalAnalyzer("(");
            TokenType token = lexer.MoveNext();
            Assert.AreEqual(TokenType.LeftParenthesis, token);
            Assert.AreEqual("(", lexer.Current);
        }

        //        [Test]
        //        public void NoTokenTest()
        //        {
        //            LexicalAnalyzer lexer = new LexicalAnalyzer(" ");
        //            TokenType token = lexer.NextToken();
        //            Assert.AreEqual(TokenType.LeftParenthesis, token);
        //        }

        [Test]
        public void QuotedStringTest()
        {
            const string tokenString = "\"Quoted string\"";
            LexicalAnalyzer lexer = new LexicalAnalyzer(tokenString);
            TokenType token = lexer.MoveNext();
            Assert.AreEqual(TokenType.QuotedString, token);
            Assert.AreEqual(tokenString, lexer.Current);
        }

        [Test]
        public void IdentityTokenTest()
        {
            LexicalAnalyzer lexer = new LexicalAnalyzer("i:user1");
            TokenType token = lexer.MoveNext();
            Assert.AreEqual(TokenType.Identity, token);
            Assert.AreEqual("i:", lexer.Current);
        }

        [Test]
        public void RoleTokenTest()
        {
            LexicalAnalyzer lexer = new LexicalAnalyzer("r:role1");
            TokenType token = lexer.MoveNext();
            Assert.AreEqual(TokenType.Role, token);
            Assert.AreEqual("r:", lexer.Current);
        }

        [Test]
        public void EofTokenTest()
        {
            LexicalAnalyzer lexer = new LexicalAnalyzer(String.Empty);
            lexer.MoveNext();
            TokenType token = lexer.MoveNext();
            Assert.AreEqual(TokenType.EndOfFile, token);
        }

        [Test]
        public void WhitespaceTest()
        {
            LexicalAnalyzer lexer = new LexicalAnalyzer("   \n\r");
            TokenType token = lexer.MoveNext();
            Assert.AreEqual(TokenType.EndOfFile, token);
        }

        [Test]
        public void ComplexExpressionTest()
        {
            string input = "((Role1 And \"Super User\" And Role3) Or Not(Role2)";
            LexicalAnalyzer lexer = new LexicalAnalyzer(input);
            ArrayList list = new ArrayList();

            for (TokenType tokenType = lexer.MoveNext(); tokenType != TokenType.EndOfFile; tokenType = lexer.MoveNext())
            {
                list.Add(new Item(tokenType, lexer.Current));
            }

            Item item = (Item)list[0];
            Assert.AreEqual(TokenType.LeftParenthesis, item.TokenType);
            Assert.AreEqual("(", item.Token);

            item = (Item)list[1];
            Assert.AreEqual(TokenType.LeftParenthesis, item.TokenType);
            Assert.AreEqual("(", item.Token);

            item = (Item)list[2];
            Assert.AreEqual(TokenType.Word, item.TokenType);
            Assert.AreEqual("Role1", item.Token);

            item = (Item)list[3];
            Assert.AreEqual(TokenType.And, item.TokenType);
            Assert.AreEqual("And", item.Token);

            item = (Item)list[4];
            Assert.AreEqual(TokenType.QuotedString, item.TokenType);
            Assert.AreEqual("\"Super User\"", item.Token);

            item = (Item)list[5];
            Assert.AreEqual(TokenType.And, item.TokenType);
            Assert.AreEqual("And", item.Token);

            item = (Item)list[6];
            Assert.AreEqual(TokenType.Word, item.TokenType);
            Assert.AreEqual("Role3", item.Token);

            item = (Item)list[7];
            Assert.AreEqual(TokenType.RightParenthesis, item.TokenType);
            Assert.AreEqual(")", item.Token);

            item = (Item)list[8];
            Assert.AreEqual(TokenType.Or, item.TokenType);
            Assert.AreEqual("Or", item.Token);

            item = (Item)list[9];
            Assert.AreEqual(TokenType.Not, item.TokenType);
            Assert.AreEqual("Not", item.Token);

            item = (Item)list[10];
            Assert.AreEqual(TokenType.LeftParenthesis, item.TokenType);
            Assert.AreEqual("(", item.Token);

            item = (Item)list[11];
            Assert.AreEqual(TokenType.Word, item.TokenType);
            Assert.AreEqual("Role2", item.Token);

            item = (Item)list[12];
            Assert.AreEqual(TokenType.RightParenthesis, item.TokenType);
            Assert.AreEqual(")", item.Token);

            Assert.AreEqual(13, list.Count);
        }

        private class Item
        {
            public Item(TokenType tokenType, string token)
            {
                this.TokenType = tokenType;
                this.Token = token;
            }

            public TokenType TokenType;
            public string Token;
        }
    }
}

#endif