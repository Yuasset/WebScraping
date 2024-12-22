# Açıklama
WebScraping ve Client olmak üzere 2 proje bulunmaktadır. Projeler birlikte çalıştırılır birbirlerine bağımlıdır. Amaç, hedef web sayfası kaynağından veri toparalamak ve bu verileri ElasticSearch adlı veritabanı sistemine kaydı gerçekleştir kullanıcılara gösterilecek bir platfom hazırlanır bu platformda veritabanından veriler çekilir ve kullanıcılara sunulur.

# Kullanılan kütüphaneler
- Nest
- HHtmlAgilityPack
- HtmlAgilityPack.CssSelectors.NetCore
- Swashbuckle.AspNetCore
* Elastic.Clients.Elasticsearch kütüphanesi üzerinde instance alınrken nesne null olarak karşılıyordu bunun yerine eski versiyonlarda kullanılan Nest kütüphanesi üzerinden ilerledim.

# Adımlar
+ WebScraping Projesi:
* Nest ile ElasticSearch bağlantı ayarları hazırlandı.
* HHtmlAgilityPack ile hedef web adresin kaynak html dokümanı çekildi.
* HtmlAgilityPack.CssSelectors.NetCore ile ilgili html dokümanı içinden verilerin bulunduğu alanlar ayıklandı.
* Bazı veriler encode formatında olduğu içinde decode yapacak bir metot hazırlandı ve veriler anlamlı hale geldi.
* Nest ile ElasticSearch elde edilen veriler kayıt edildi.
+ Client Projesi:
* Nest ile ElasticSearch bağlantı ayarları hazırlandı ve veriler çekildi
* Bootstrap ile card tipi arayüz hazırlandı veriler bu arayüze entegre edildi
* arama kutusu eklendi ve javascript ile filtreleme işlevi kazandırıldı.
