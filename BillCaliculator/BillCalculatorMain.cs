using System;
using System.Collections.Generic;
using System.Collections;

namespace BillCalculator
{
    class MainClass
    {
        public static void Main(string[] args)
        {


            ArrayList Bag1 = new ArrayList();
            Bag1.Add(new Item(1, "book", 12.49, true));
            Bag1.Add(new Item(2, "music CD", 14.99, false));
            Bag1.Add(new Item(3, "chocolate bar", 0.85, true));

            PrintSalesReceipt(Bag1);

            ArrayList Bag2 = new ArrayList();
            Bag2.Add(new Item(4, "imported box of chocolates", 10, true, true));
            Bag2.Add(new Item(5, "imported bottle of perfume", 47.50, false, true));

            PrintSalesReceipt(Bag2);

            ArrayList Bag3 = new ArrayList();
            Bag3.Add(new Item(6, "imported bottle of perfume", 27.99, false, true));
            Bag3.Add(new Item(7, "bottle of perfume", 18.99, false, false));
            Bag3.Add(new Item(8, "packet of headache pills", 9.75, true));
            Bag3.Add(new Item(9, "imported box of chocolates", 11.25, true, true));

            PrintSalesReceipt(Bag3);
            Console.ReadKey();


        }

        private static void PrintSalesReceipt(IList bag)
        {

            double salesTax = 0;
            double totalPrice = 0;
            foreach (Item item in bag)
            {
                //Console.WriteLine("Item" + item);
                SalesTaxCalulator salesTaxCalc = new SalesTaxCalulator();

                salesTaxCalc.ApplyTax(item);
                salesTax += item.SalesTax;
                totalPrice += item.FinalPrice;

                Console.WriteLine("{0}: {1}", item.Description, item.FinalPrice);
            }
            Console.WriteLine("Sales Taxs: {0}", salesTax);
            Console.WriteLine("Total: {0}", totalPrice);

            Console.WriteLine("******************");

        }
    }

    class Item
    {
        int itemId;
        string description;
        double price;

        double salesTaxPercentage;
        double importTaxPercentage;

        double appliedSalesTax;
        double appliedImportTax;

        double finalPrice;

        bool isImported;
        bool isTaxExempt;

        public int Id
        {
            get
            {
                return this.itemId;
            }
            set
            {
                this.itemId = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public double Price
        {
            get
            {
                return this.price;
            }
            /*set
            {
                this.price = value;
            }*/
        }

        public double SalesTaxPercent
        {
            get
            {
                return salesTaxPercentage;
            }
            set
            {
                this.salesTaxPercentage = value;
            }
        }

        public double ImportTaxPercent
        {
            get
            {
                return this.importTaxPercentage;
            }
            set
            {
                this.importTaxPercentage = value;
            }
        }

        public double SalesTax
        {
            get
            {
                return this.appliedSalesTax;
            }
            set
            {
                this.appliedSalesTax = value;
            }
        }

        public double ImportTax
        {
            get
            {
                return this.appliedImportTax;
            }
            set
            {
                this.appliedImportTax = value;
            }
        }

        public double FinalPrice
        {
            get
            {
                return this.finalPrice;
            }
            set
            {
                this.finalPrice = value;
            }
        }

        public bool IsImported
        {
            get
            {
                return this.isImported;
            }
        }

        public bool IsTaxExempt
        {
            get
            {
                return this.isTaxExempt;
            }
        }

        public Item(int id, string itemDescription, double price)
        {
            this.itemId = id;
            this.description = itemDescription;
            this.price = price;
            this.isImported = false;
        }
        public Item(int id, string itemDescription, double price, bool isTaxExempt)
        {
            this.itemId = id;
            this.description = itemDescription;
            this.price = price;
            this.isImported = false;
            this.isTaxExempt = isTaxExempt;
        }

        public Item(int id, string itemDescription, double price, bool isTaxExempt, bool isImported)
        {
            this.itemId = id;
            this.description = itemDescription;
            this.price = price;
            this.isImported = isImported;
            this.isTaxExempt = isTaxExempt;
        }

        public override string ToString()
        {
            return "{itemId : " + this.itemId
                + "}{description : " + this.description
                + "}{price : " + this.price
              + "}{salesTax : " + this.appliedSalesTax
              + "}{importTax : " + this.appliedImportTax
              + "}{finalPrice : " + this.finalPrice
                + "}"

                ;
        }
    }



    interface ITaxCalculator
    {
        Item ApplyTax(Item item);
        Item ApplySalesTax(Item item);
        Item ApplyImportTax(Item item);
    }

    class SalesTaxCalulator : ITaxCalculator
    {


        const double defaultSalesTaxPercent = 0.1;
        const double defaultImportTaxPercent = 0.05;

        public SalesTaxCalulator()
        {


        }
        public Item ApplyTax(Item item)
        {
            item = this.ApplySalesTax(item);
            item = this.ApplyImportTax(item);

            item.SalesTax = SalesTaxCalulator.applyTax(item.Price, item.SalesTaxPercent + item.ImportTaxPercent);
            item.FinalPrice = item.Price + item.SalesTax + item.ImportTax;
            return item;
        }

        public Item ApplySalesTax(Item item)
        {
            double salesTax = 0;
            if (!item.IsTaxExempt)
            {
                salesTax = SalesTaxCalulator.defaultSalesTaxPercent;
            }
            item.SalesTaxPercent = salesTax;

            //item.SalesTax = SalesTaxCalulator.applyTax(item.Price, item.SalesTaxPercent);
            return item;
        }

        private static double applyTax(double price, double tax)
        {

            double appliedTax = Math.Round(price * tax, 2, MidpointRounding.AwayFromZero);


            int reminder = Reminder((int)(appliedTax * 100));
            //Console.WriteLine("round Tax :", roundTax);

            if (reminder > 0 && reminder < 5)
            {
                appliedTax = appliedTax + (5 - reminder) * .01;
            }
            else if (reminder > 5)
            {

                appliedTax = appliedTax + (10 - reminder) * .01;
            }


            // Console.WriteLine("{0} => {1}",appliedTax,  Reminder((int)(appliedTax * 100)));


            return appliedTax;

        }


        private static int Reminder(int value)
        {
            int result;
            //Console.WriteLine(value);

            result = value % 10;
            return result;
        }



        public Item ApplyImportTax(Item item)
        {

            /* if (this.importTaxApplicable.TryGetValue(item.Id, out importTax))
             {
                 item.ImportTaxPercent = importTax;
             }
             else
             {
                 item.ImportTaxPercent = SalesTaxCalulator.defaultImportTax;
             }*/
            double importTax = 0;
            if (item.IsImported)
            {
                importTax = SalesTaxCalulator.defaultImportTaxPercent;
            }

            item.ImportTaxPercent = importTax;
            //item.ImportTax = SalesTaxCalulator.applyTax(item.Price, item.ImportTaxPercent);
            return item;
        }
    }


}