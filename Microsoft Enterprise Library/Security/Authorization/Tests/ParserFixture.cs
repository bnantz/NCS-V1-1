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
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Authorization.Tests
{
    [TestFixture]
    public class ParserFixture
    {
        [Test]
        public void AndExpressionTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("(R:Role1 and R:Role2)");
            Assert.AreEqual(typeof(AndOperator), expression.GetType());
            AndOperator andOperator = (AndOperator)expression;
            Assert.IsNotNull(andOperator.Left);
            Assert.IsNotNull(andOperator.Right);
            Assert.AreEqual(typeof(RoleExpression), andOperator.Left.GetType());
            Assert.AreEqual(typeof(RoleExpression), andOperator.Right.GetType());
            Assert.AreEqual("Role1", ((RoleExpression)andOperator.Left).Word.Value);
            Assert.AreEqual("Role2", ((RoleExpression)andOperator.Right).Word.Value);
        }

        [Test]
        public void AndExpressionWithoutParentheses()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("R:Role1 and R:Role2");
            Assert.AreEqual(typeof(AndOperator), expression.GetType());
            AndOperator andOperator = (AndOperator)expression;
            Assert.IsNotNull(andOperator.Left);
            Assert.IsNotNull(andOperator.Right);
            Assert.AreEqual(typeof(RoleExpression), andOperator.Left.GetType());
            Assert.AreEqual(typeof(RoleExpression), andOperator.Right.GetType());
            Assert.AreEqual("Role1", ((RoleExpression)andOperator.Left).Word.Value);
            Assert.AreEqual("Role2", ((RoleExpression)andOperator.Right).Word.Value);
        }

        [Test]
        public void MultipleAndExpressionWithParentheses()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("R:Role1 and R:Role2 and I:User1");
            Assert.AreEqual(typeof(AndOperator), expression.GetType());
            AndOperator andOperator = (AndOperator)expression;
            Assert.IsNotNull(andOperator.Left);
            Assert.IsNotNull(andOperator.Right);
            Assert.AreEqual(typeof(AndOperator), andOperator.Left.GetType());
            Assert.AreEqual(typeof(IdentityExpression), andOperator.Right.GetType());
            Assert.AreEqual("User1", ((IdentityExpression)andOperator.Right).Word.Value);

            AndOperator leftAndOperator = (AndOperator)andOperator.Left;
            Assert.AreEqual("Role2", ((RoleExpression)leftAndOperator.Right).Word.Value);
        }

        [Test]
        public void IdentityExpressionTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("I:Bob");
            Assert.AreEqual(typeof(IdentityExpression), expression.GetType());
            IdentityExpression identityExpression = (IdentityExpression)expression;
            Assert.AreEqual(typeof(WordExpression), identityExpression.Word.GetType());
            Assert.AreEqual("Bob", identityExpression.Word.Value);
        }

        [Test]
        public void NotExpressionTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("Not I:Bob");
            Assert.AreEqual(typeof(NotOperator), expression.GetType());
            NotOperator notOperator = (NotOperator)expression;
            Assert.AreEqual(typeof(IdentityExpression), notOperator.Expression.GetType());
            IdentityExpression identityExpression = (IdentityExpression)notOperator.Expression;
            Assert.AreEqual(typeof(WordExpression), identityExpression.Word.GetType());
            Assert.AreEqual("Bob", identityExpression.Word.Value);
        }

        [Test]
        public void RoleExpressionTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("R:Managers");
            Assert.AreEqual(typeof(RoleExpression), expression.GetType());
            RoleExpression roleExpression = (RoleExpression)expression;
        }

        [Test]
        public void AndNotTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("R:Managers AND NOT I:Bob");
            Assert.AreEqual(typeof(AndOperator), expression.GetType());
            AndOperator andOperator = (AndOperator)expression;
            Assert.AreEqual(typeof(RoleExpression), andOperator.Left.GetType());
            RoleExpression roleExpression = (RoleExpression)andOperator.Left;
            Assert.AreEqual("Managers", roleExpression.Word.Value);
            Assert.AreEqual(typeof(NotOperator), andOperator.Right.GetType());
            NotOperator notOperator = (NotOperator)andOperator.Right;
            Assert.AreEqual(typeof(IdentityExpression), notOperator.Expression.GetType());
            IdentityExpression identityExpression = (IdentityExpression)notOperator.Expression;
            Assert.AreEqual("Bob", identityExpression.Word.Value);

        }

        [Test]
        public void OrExpressionTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("(R:Role1 or R:Role2)");
            Assert.AreEqual(typeof(OrOperator), expression.GetType());
            OrOperator andOperator = (OrOperator)expression;
            Assert.IsNotNull(andOperator.Left);
            Assert.IsNotNull(andOperator.Right);
            Assert.AreEqual(typeof(RoleExpression), andOperator.Left.GetType());
            Assert.AreEqual(typeof(RoleExpression), andOperator.Right.GetType());
            Assert.AreEqual("Role1", ((RoleExpression)andOperator.Left).Word.Value);
            Assert.AreEqual("Role2", ((RoleExpression)andOperator.Right).Word.Value);
        }

        [Test]
        public void AndOrNotTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse(
                "(R:HumanResources OR R:GeneralManagers) AND NOT R:HRSpecialist");
            Assert.AreEqual(typeof(AndOperator), expression.GetType());
            AndOperator andOperator = (AndOperator)expression;
            Assert.AreEqual(typeof(OrOperator), andOperator.Left.GetType());
            OrOperator orOperator = (OrOperator)andOperator.Left;
            Assert.AreEqual(typeof(RoleExpression), orOperator.Left.GetType());
            RoleExpression hrRoleExpression = (RoleExpression)orOperator.Left;
            Assert.AreEqual("HumanResources", hrRoleExpression.Word.Value);
            Assert.AreEqual(typeof(RoleExpression), orOperator.Right.GetType());
            RoleExpression gmRoleExpression = (RoleExpression)orOperator.Right;
            Assert.AreEqual("GeneralManagers", gmRoleExpression.Word.Value);
            Assert.AreEqual(typeof(NotOperator), andOperator.Right.GetType());
            NotOperator notOperator = (NotOperator)andOperator.Right;
            Assert.AreEqual(typeof(RoleExpression), notOperator.Expression.GetType());
            RoleExpression specialistRoleExpression = (RoleExpression)notOperator.Expression;
            Assert.AreEqual("HRSpecialist", specialistRoleExpression.Word.Value);
        }

        [Test]
        public void AndOrNotTestWithQuotedStrings()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse(
                "(R:\"Human Resources\" OR R:\"General Managers\") AND NOT R:\"HR Specialist\"");
            Assert.AreEqual(typeof(AndOperator), expression.GetType());
            AndOperator andOperator = (AndOperator)expression;
            Assert.AreEqual(typeof(OrOperator), andOperator.Left.GetType());
            OrOperator orOperator = (OrOperator)andOperator.Left;
            Assert.AreEqual(typeof(RoleExpression), orOperator.Left.GetType());
            RoleExpression hrRoleExpression = (RoleExpression)orOperator.Left;
            Assert.AreEqual("Human Resources", hrRoleExpression.Word.Value);
            Assert.AreEqual(typeof(RoleExpression), orOperator.Right.GetType());
            RoleExpression gmRoleExpression = (RoleExpression)orOperator.Right;
            Assert.AreEqual("General Managers", gmRoleExpression.Word.Value);
            Assert.AreEqual(typeof(NotOperator), andOperator.Right.GetType());
            NotOperator notOperator = (NotOperator)andOperator.Right;
            Assert.AreEqual(typeof(RoleExpression), notOperator.Expression.GetType());
            RoleExpression specialistRoleExpression = (RoleExpression)notOperator.Expression;
            Assert.AreEqual("HR Specialist", specialistRoleExpression.Word.Value);
        }

        [Test]
        public void NotAnonymousTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("Not I:?");
            Assert.AreEqual(typeof(NotOperator), expression.GetType());
            NotOperator notOperator = (NotOperator)expression;
            Assert.AreEqual(typeof(IdentityExpression), notOperator.Expression.GetType());
            IdentityExpression identityExpression = (IdentityExpression)notOperator.Expression;
            Assert.AreEqual(typeof(AnonymousExpression), identityExpression.Word.GetType());
        }

        [Test]
        public void MissingRightParenthesisTest()
        {
            ExpectSyntaxError("(R:Role1", 7);
        }

        [Test]
        public void MissingOperandTest()
        {
            ExpectSyntaxError("And And", 0);
        }

        [Test]
        public void RepeatingOperatorTest()
        {
            ExpectSyntaxError("R:Role1 Or OR R:Role2", 9);
        }

        [Test]
        public void UnqualifiedWordTest()
        {
            ExpectSyntaxError("Not User1", 2);
        }

        private void ExpectSyntaxError(string expression, int index)
        {
            Parser parser = new Parser();
            try
            {
                parser.Parse(expression);
                Assert.Fail();
            }
            catch (SyntaxException e)
            {
                Assert.AreEqual(index, e.Index);
            }
        }
    }
}

#endif