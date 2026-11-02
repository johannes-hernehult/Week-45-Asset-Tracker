Console.WriteLine("======= Asset Tracker =======");

/*
 * Plan
 * - Simple UML diagram of classes
 * - Data structures
 * - List requirements
 * -
 */

/*
 * === Requirements ===
 * - Lifespan 3 years
 * - Colored feedback on near / past lifespan items
 *     - < 3 months remaining = red
 *     - > 3 && < 6 months remaining = yellow
 *     - > 3 years = red and add '!' at the end of 'RED' status
 * - Assets belong to different offices in different countries
 * - Each office uses its own currency.
 *     - Think through how to do this as all should have USD and local currency in data structure
 * - Be able to sort by AssetType, PurchaseDate or Office
 *     - 'a' & 'd'
 * -
 */

/*
 * === Data Structure ===
 * Asset:
 *    - Id
 *    - Brand
 *    - Model
 *    - Purchase date
 *    - PriceUSD
 *    - PriceLocal
 *    - Office
 *    - AssetType: abstract string
 *
 * Office:
 *    - Id
 *    - Name
 *    - Country
 *    - City
 *    - CurrencyToken
 *    - ExchangeRate
 *
 * Computer : Asset:
 *   - AssetType: override "Computer"
 * MobilePhone : Asset:
 * *   - AssetType: override "Mobile Phone"
 * Tablet : Asset:
 * *   - AssetType: override "Tablet"
 */