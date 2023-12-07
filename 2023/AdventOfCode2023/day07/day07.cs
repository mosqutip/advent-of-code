/*
--- Day 7: Camel Cards ---
Your all-expenses-paid trip turns out to be a one-way, five-minute ride in an airship. (At least it's a cool airship!) It drops you off at the edge of a vast desert and descends back to Island Island.

"Did you bring the parts?"

You turn around to see an Elf completely covered in white clothing, wearing goggles, and riding a large camel.

"Did you bring the parts?" she asks again, louder this time. You aren't sure what parts she's looking for; you're here to figure out why the sand stopped.

"The parts! For the sand, yes! Come with me; I will show you." She beckons you onto the camel.

After riding a bit across the sands of Desert Island, you can see what look like very large rocks covering half of the horizon. The Elf explains that the rocks are all along the part of Desert Island that is directly above Island Island, making it hard to even get there. Normally, they use big machines to move the rocks and filter the sand, but the machines have broken down because Desert Island recently stopped receiving the parts they need to fix the machines.

You've already assumed it'll be your job to figure out why the parts stopped when she asks if you can help. You agree automatically.

Because the journey will take a few days, she offers to teach you the game of Camel Cards. Camel Cards is sort of similar to poker except it's designed to be easier to play while riding a camel.

In Camel Cards, you get a list of hands, and your goal is to order them based on the strength of each hand. A hand consists of five cards labeled one of A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, or 2. The relative strength of each card follows this order, where A is the highest and 2 is the lowest.

Every hand is exactly one type. From strongest to weakest, they are:

    Five of a kind, where all five cards have the same label: AAAAA
    Four of a kind, where four cards have the same label and one card has a different label: AA8AA
    Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
    Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
    Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
    One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
    High card, where all cards' labels are distinct: 23456

Hands are primarily ordered based on type; for example, every full house is stronger than any three of a kind.

If two hands have the same type, a second ordering rule takes effect. Start by comparing the first card in each hand. If these cards are different, the hand with the stronger first card is considered stronger. If the first card in each hand have the same label, however, then move on to considering the second card in each hand. If they differ, the hand with the higher second card wins; otherwise, continue with the third card in each hand, then the fourth, then the fifth.

So, 33332 and 2AAAA are both four of a kind hands, but 33332 is stronger because its first card is stronger. Similarly, 77888 and 77788 are both a full house, but 77888 is stronger because its third card is stronger (and both hands have the same first and second card).

To play Camel Cards, you are given a list of hands and their corresponding bid (your puzzle input). For example:

32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483

This example shows five hands; each hand is followed by its bid amount. Each hand wins an amount equal to its bid multiplied by its rank, where the weakest hand gets rank 1, the second-weakest hand gets rank 2, and so on up to the strongest hand. Because there are five hands in this example, the strongest hand will have rank 5 and its bid will be multiplied by 5.

So, the first step is to put the hands in order of strength:

    32T3K is the only one pair and the other hands are all a stronger type, so it gets rank 1.
    KK677 and KTJJT are both two pair. Their first cards both have the same label, but the second card of KK677 is stronger (K vs T), so KTJJT gets rank 2 and KK677 gets rank 3.
    T55J5 and QQQJA are both three of a kind. QQQJA has a stronger first card, so it gets rank 5 and T55J5 gets rank 4.

Now, you can determine the total winnings of this set of hands by adding up the result of multiplying each hand's bid with its rank (765 * 1 + 220 * 2 + 28 * 3 + 684 * 4 + 483 * 5). So the total winnings in this example are 6440.

Find the rank of every hand in your set. What are the total winnings?

Your puzzle answer was 251121738.

--- Part Two ---
To make things a little more interesting, the Elf introduces one additional rule. Now, J cards are jokers - wildcards that can act like whatever card would make the hand the strongest type possible.

To balance this, J cards are now the weakest individual cards, weaker even than 2. The other cards stay in the same order: A, K, Q, T, 9, 8, 7, 6, 5, 4, 3, 2, J.

J cards can pretend to be whatever card is best for the purpose of determining hand type; for example, QJJQ2 is now considered four of a kind. However, for the purpose of breaking ties between two hands of the same type, J is always treated as J, not the card it's pretending to be: JKKK2 is weaker than QQQQ2 because J is weaker than Q.

Now, the above example goes very differently:

32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483

    32T3K is still the only one pair; it doesn't contain any jokers, so its strength doesn't increase.
    KK677 is now the only two pair, making it the second-weakest hand.
    T55J5, KTJJT, and QQQJA are now all four of a kind! T55J5 gets rank 3, QQQJA gets rank 4, and KTJJT gets rank 5.

With the new joker rule, the total winnings in this example are 5905.

Using the new joker rule, find the rank of every hand in your set. What are the new total winnings?

Your puzzle answer was 251421071.
*/

using AdventOfCode2023;

enum HandType
{
    HighCard = 0,
    OnePair = 1,
    TwoPair = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6
}

class Day07()
{
    private static List<string> inputFileLines = Utilites.ReadInputFile("07");

    private static int solvePartOne()
    {
        List<Tuple<int, string, int>> hands = new List<Tuple<int, string, int>>();
        foreach (var line in inputFileLines)
        {
            var values = line.Split(' ');
            var hand = values[0];
            var bid = values[1];

            int[] cards = new int[13];
            foreach (var card in hand)
            {
                switch (char.ToLower(card))
                {
                    case 'a':
                        cards[0]++;
                        break;
                    case 'k':
                        cards[1]++;
                        break;
                    case 'q':
                        cards[2]++;
                        break;
                    case 'j':
                        cards[3]++;
                        break;
                    case 't':
                        cards[4]++;
                        break;
                    case '9':
                        cards[5]++;
                        break;
                    case '8':
                        cards[6]++;
                        break;
                    case '7':
                        cards[7]++;
                        break;
                    case '6':
                        cards[8]++;
                        break;
                    case '5':
                        cards[9]++;
                        break;
                    case '4':
                        cards[10]++;
                        break;
                    case '3':
                        cards[11]++;
                        break;
                    case '2':
                        cards[12]++;
                        break;
                    default:
                        break;
                }
            }

            int pairCount = 0;
            foreach (var cardCount in cards)
            {
                if (cardCount == 2)
                {
                    pairCount++;
                }
            }

            HandType handType;
            if (cards.Contains(5))
            {
                handType = HandType.FiveOfAKind;
            }
            else if (cards.Contains(4))
            {
                handType = HandType.FourOfAKind;
            }
            else if (cards.Contains(3) && cards.Contains(2))
            {
                handType = HandType.FullHouse;
            }
            else if (cards.Contains(3))
            {
                handType = HandType.ThreeOfAKind;
            }
            else if (pairCount == 2)
            {
                handType = HandType.TwoPair;
            }
            else if (pairCount == 1)
            {
                handType = HandType.OnePair;
            }
            else
            {
                handType = HandType.HighCard;
            }

            var ordinalHand = hand.Replace('A', 'Z');
            ordinalHand = ordinalHand.Replace('K', 'Y');
            ordinalHand = ordinalHand.Replace('Q', 'X');
            ordinalHand = ordinalHand.Replace('J', 'W');
            ordinalHand = ordinalHand.Replace('T', 'V');

            Tuple<int, string, int> handInfo = new Tuple<int, string, int>((int)handType, ordinalHand, int.Parse(bid));
            hands.Add(handInfo);
        }

        hands = [.. hands
            .OrderBy(el => el.Item1)
            .ThenBy(el => el.Item2)
            .ThenBy(el => el.Item3)];

        int sum = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            sum += (hands[i].Item3 * (i + 1));
        }

        return sum;
    }

    private static int solvePartTwo()
    {
        List<Tuple<int, string, int>> hands = new List<Tuple<int, string, int>>();
        foreach (var line in inputFileLines)
        {
            var values = line.Split(' ');
            var hand = values[0];
            var bid = values[1];

            int[] cards = new int[12];
            int jokers = 0;
            foreach (var card in hand)
            {
                switch (char.ToLower(card))
                {
                    case 'j':
                        jokers++;
                        break;
                    case 'a':
                        cards[0]++;
                        break;
                    case 'k':
                        cards[1]++;
                        break;
                    case 'q':
                        cards[2]++;
                        break;
                    case 't':
                        cards[3]++;
                        break;
                    case '9':
                        cards[4]++;
                        break;
                    case '8':
                        cards[5]++;
                        break;
                    case '7':
                        cards[6]++;
                        break;
                    case '6':
                        cards[7]++;
                        break;
                    case '5':
                        cards[8]++;
                        break;
                    case '4':
                        cards[9]++;
                        break;
                    case '3':
                        cards[10]++;
                        break;
                    case '2':
                        cards[11]++;
                        break;
                    default:
                        break;
                }
            }

            int pairCount = 0;
            foreach (var cardCount in cards)
            {
                if (cardCount == 2)
                {
                    pairCount++;
                }
            }

            HandType handType;
            if (cards.Contains(5))
            {
                handType = HandType.FiveOfAKind;
            }
            else if (cards.Contains(4))
            {
                handType = HandType.FourOfAKind;
            }
            else if (cards.Contains(3) && cards.Contains(2))
            {
                handType = HandType.FullHouse;
            }
            else if (cards.Contains(3))
            {
                handType = HandType.ThreeOfAKind;
            }
            else if (pairCount == 2)
            {
                handType = HandType.TwoPair;
            }
            else if (pairCount == 1)
            {
                handType = HandType.OnePair;
            }
            else
            {
                handType = HandType.HighCard;
            }

            // For every joker in the hand, increase the rank of the hand
            // according to its current rank:
            //   - high card:       one pair (+1)
            //   - one pair:        three-of-a-kind (+2)
            //   - two pair:        full house (+2)
            //   - three-of-a-kind: four-of-a-kind (+2)
            //   - full house:      four-of-a-kind (+1)
            //   - four-of-a-kind:  five-of-a-kind (+1)
            //   - five-of-a-kind:  cannot get better
            for (int i = 0; i < jokers; i++)
            {
                switch (handType)
                {
                    case HandType.HighCard:
                        handType++;
                        break;
                    case HandType.OnePair:
                        handType += 2;
                        break;
                    case HandType.TwoPair:
                        handType += 2;
                        break;
                    case HandType.ThreeOfAKind:
                        handType += 2;
                        break;
                    case HandType.FullHouse:
                        handType++;
                        break;
                    case HandType.FourOfAKind:
                        handType++;
                        break;
                    default:
                        break;
                }
            }

            var ordinalHand = hand.Replace('A', 'Z');
            ordinalHand = ordinalHand.Replace('K', 'Y');
            ordinalHand = ordinalHand.Replace('Q', 'X');
            ordinalHand = ordinalHand.Replace('T', 'V');

            // Jokers become lower than even '2'
            ordinalHand = ordinalHand.Replace('J', '0');

            Tuple<int, string, int> handInfo = new Tuple<int, string, int>((int)handType, ordinalHand, int.Parse(bid));
            hands.Add(handInfo);
        }

        hands = [.. hands
            .OrderBy(el => el.Item1)
            .ThenBy(el => el.Item2)
            .ThenBy(el => el.Item3)];

        int sum = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            sum += (hands[i].Item3 * (i + 1));
        }

        return sum;
    }

    public static void solve()
    {
        Console.WriteLine($"Part 1: {solvePartOne()}");
        Console.WriteLine($"Part 2: {solvePartTwo()}");
    }
}