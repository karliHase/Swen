using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MCTG.Ingame.Cards;

namespace MTCG.Test
{
    public class CardTest
    {
        [Fact]
        public void test_Multiplier_fireMonster_vs_waterMonster()
        {
            //Arrange
            Card fireM = new Card("asdg", 10.0, "FireKraken");

            Card waterM = new Card("b", 10, "WaterKraken");

            //Act
            double attackFireM = fireM.Attack(waterM);
            double attackWaterM = waterM.Attack(fireM);

            //Assert
            Assert.Equal(10, attackWaterM);
            Assert.Equal(10, attackFireM);

        }

        [Theory]
        [InlineData("FireDragon", "WaterSpell")]
        [InlineData("RegularOrk", "FireSpell")]
        [InlineData("WaterDragon", "NormalSpell")]
        public void attack_of_spellCard_should_double(string nameM, string nameS)
        {
            //Arrange
            ICardInterface spell = new Card("a", 10, nameS);
            ICardInterface monster = new Card("b",10, nameM);

            //Aact
            double attackS = spell.Attack(monster);
            bool result = (spell.GetDmg() * 2 == attackS);
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("WaterDragon", "FireSpell")]
        [InlineData("NormalOrk", "WaterSpell")]
        [InlineData("FireDragon", "NormalSpell")]
        public void attack_of_monsterCard_should_double(string nameM, string nameS)
        {
            //Arrange
            ICardInterface spell = new Card("a", 10, nameS);
            ICardInterface monster = new Card("b", 10, nameM);

            //Aact
            double attackM = monster.Attack(spell);
            bool result = (monster.GetDmg() * 2 == attackM);
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("FireDragon", "WaterSpell")]
        [InlineData("NormalOrk", "FireSpell")]
        [InlineData("WaterDragon", "NormalSpell")]
        public void attack_of_monsterCard_should_halve(string nameM, string nameS)
        {
            //Arrange
            ICardInterface spell = new Card("a", 10, nameS);
            ICardInterface monster = new Card("b", 10, nameM);

            //Aact
            double attackM = monster.Attack(spell);
            bool result = (monster.GetDmg() / 2 == attackM);
            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("WaterDragon", "FireSpell")]
        [InlineData("NormalOrk", "WaterSpell")]
        [InlineData("FireDragon", "NormalSpell")]
        public void attack_of_spellCard_should_halve(string nameM, string nameS)
        {
            //Arrange
            ICardInterface spell = new Card("a", 10, nameS);
            ICardInterface monster = new Card("b", 10, nameM);

            //Aact
            double attackS = spell.Attack(monster);
            bool result = (spell.GetDmg() / 2 == attackS);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void drowning_Knight()
        {
            //Arrange
            ICardInterface knight = new Card("a", 10, "FireKnight");
            ICardInterface waterSpell = new Card("b", 1, "WaterSpell");
            //Act
            double attackKnight = knight.Attack(waterSpell);
            //Assert
            Assert.Equal(0, attackKnight);
        }

        [Fact]
        public void ork_vs_wizard()
        {
            //Arrange
            ICardInterface wizard = new Card("a", 10, "FireWizard");
            ICardInterface ork = new Card("b", 20, "WaterOrk");

            //Act
            double attackOrk = ork.Attack(wizard);
            //Assert
            Assert.Equal(0, attackOrk);
        }

        [Fact]
        public void goblin_vs_dragon()
        {
            //Arrange
            ICardInterface goblin = new Card("a", 10, "FireGoblin");
            ICardInterface dragon = new Card("b", 1, "WaterDragon");

            //Act
            double attackGoblin = goblin.Attack(dragon);
            //Assert
            Assert.Equal(0, attackGoblin);
        }

        [Fact]
        public void fireElve_vs_dragon()
        {
            //Arrange
            ICardInterface elve = new Card("a", 10, "FireElve");
            ICardInterface dragon = new Card("b",20, "FireDragon");

            //Act
            double attackDragon = dragon.Attack(elve);
            //Assert
            Assert.Equal(0, attackDragon);
        }
    }
}

