1. How would you organize this data in object storage?

Assume some root level <root>/, with prefixes for <year> and <source-system> (ie, <root>/2025/Bloomberg/). These prefixes allow for clean partitioning in S3. 

Individual file names should have the full date followed by filename. The date (and year of the storage prefix) should reflect the as_of_date for the data (I'm assuming this daily data reflects a single 24 hour period for some timezone Z, such that the as_of_date is consistent across all rows for that file). If its possible to have multiple files of the same name (ie, Bloomberg issues a corrected file a day later, and this file can be considered "version 2"), we want to include a suffix to the filename to indicate file "version". The suffix will be a date-timestamp of when the file was added to the bucket (having it in the filename just makes programmatic sorting easier), and (ideally) in parquet format for easier processing.

Thus a sample 'S3' path will look like: "<root>/2025/Bloomberg/20250102_HoldingsAll_20250103-085222Z.pqt"

2. How would you design the analytical tables?

I would create a partition key based on as_of_date. Given a daily volume of 2M rows (500M/252 trading days per year), I believe that should be sufficient based on my experience with AWS Redshift.

I'm not familiar enough with distribution and bucketing strategies to comment, but given that most queries should be client-specific I would use that key to distrubute across nodes.

I should add that in this data I would also include a "file_name" column that can serve as a duplicate/primary row filter. Occasionally end-users want to view duplicate rows to see how corrections were applied, and having a filename that can be timestamp sorted helps to get the latest file if needed. 

Indicies should thus be applied across as_of_date, client_id, file_name, and lastly fund_id. Depending on the number of metric-values present that may be indexed as well.

3. How should APIs be designed to work efficiently with an MPP system?

I think this largely depends on what stakeholders are expecting as a response to the API. Obviously, we want to limit the size of the data they're querying to keep responses manageable. The API should having cacheing system (either  db systems query caching or an external cache) in place if there are common queries across multiple users (ie from my experience, yesterdays market notion total for a given portfolio). Pagination should be implemented to limit the response size on order of latency. Lastly, columnar filters should be explicit as possible to avoid returning full data sets for tables, and unnecessary columns should be excluded from the resultant set.

Materialized views are also useful in denormalizing data for common queries. 


4. What failure modes do you expect, and how would you mitigate them?
 
 The primary failure I can think of is query latency due to locks. In OLTP data applications I've worked on, I've handled this by implementing a CQRS db pattern. I'm not familiar with the correct design approachs for an OLAP lakehouse. Adding more nodes is the first step I'd think of but I'd imagine it isn't cost effective.

 5. Assume data volume grows **10x over two yeras**. What breaks first? What would you change?

 With 5B rows per year I'd expect additional nodes to be required due to storage capacity, and additional paritioning to be needed to facilitate querying (2M rows per day becomes 20M, so this should be subdivided further). I expect additional painpoints to exist, but lack the expereince of having to deal with such scaling. 