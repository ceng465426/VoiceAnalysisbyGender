<!DOCTYPE html>
<html>

<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Welcome file</title>
  <link rel="stylesheet" href="https://stackedit.io/style.css" />
</head>

<body class="stackedit">
  <div class="stackedit__left">
    <div class="stackedit__toc">
      
<ul>
<li><a href="#giriş">Giriş</a></li>
<li><a href="#cinsiyete-dayalı-ses-analizi">Cinsiyete Dayalı Ses Analizi</a>
<ul>
<li><a href="#kullanılan-yöntem-diagramları">Kullanılan Yöntem Diagramları</a></li>
</ul>
</li>
</ul>

    </div>
  </div>
  <div class="stackedit__right">
    <div class="stackedit__html">
      <h1 id="giriş">Giriş</h1>
<p><strong>Konuşma</strong>, sahip olduğu zengin boyutlu karakterinden dolayı insanlar arasındaki iletişimin en kolay ve doğal şeklidir. Konuşma sırasında dinleyiciye yalnızca kelimeler iletilmez. Aynı zamanda konuşmacı hakkında <strong>kimlik</strong>, <strong>yaş</strong>, <strong>cinsiyet</strong>, <strong>ruh hali</strong> gibi bilgilerde iletilir. İnsanlar arasındaki bu iletişimin bilgisayarla da kurulması için yoğun çalışmalar yapılmaktadır. Bu bilgiler değişik amaçlar için kullanılır. Özellikle biyometrik sistemlerde sesin kullanımı hem maliyet hem de kullanım kolaylığı açısından avantaj sağlamaktadır.</p>
<p>Örneğin ortama yerleştirilen bir mikrofon aracılığıyla kişinin haberi bile olmadan ses kaydı alınabilir ve sistem uygulanabilir. Bu tarz sistemler savunma sektöründe hem özel hem de devletler tarafından kullanılmaktadır. Ayrıca uzaktan erişim kolaylığı da ses biyometrisinin avantajlarından bir diğeridir. Bu çalışmada ses dalgasının içinde barındırdığı kişisel bilgilerden konuşmacı cinsiyetinin otomatik olarak belirlenmesi amaçlanmaktadır.</p>
<h1 id="cinsiyete-dayalı-ses-analizi">Cinsiyete Dayalı Ses Analizi</h1>
<p>Bu çalışmada konuşmacının cinsiyet grubunun otomatik olarak belirlenmesi ve ses analizi konusu ele alınmıştır.</p>
<blockquote>
<p>Kullanım Alanları</p>
</blockquote>
<ul>
<li>Başta ticari medikal ve adli olmak üzere geniş bir uygulama alanına sahip olan otomatik yaş ve cinsiyet tanıma sistemleri doğrudan bir servisin seçiminde kullanılabileceği gibi farklı tanıma sistemlerinde ön işlem olarak da kullanılır.</li>
</ul>
<p>Ancak konuşma sinyali, görüntü sinyali gibi ayırt edilebilecek faktör sayısı azdır ve işlenmesi çok daha zordur. Konuşma sinyali oldukça değişkendir ve başarılı bir sistemin gerçekleştirilmesi için konuşmayı etkileyen tüm faktörlerin değerlendirilmesi gerekir. Bu çalışmada konuşmacı cinsiyetinin <strong>metinden bağımsız</strong> olarak belirlenmesi amaçlanmaktadır.</p>
<blockquote>
<p>Önerilen sistem iki bölümden oluşmaktadır.</p>
</blockquote>
<ul>
<li>Birinci bölüm olan <strong>eğitim</strong> aşamasında deneklerden alınan ses kayıtlarından öznitelik vektörü hesaplanır.<br>
Çalışmada öznitelik vektörü olarak <strong>MFCC(Mel Frequency Cepstral Coefficients)</strong> kullanılmıştır. Elde edilen MFCC öznitelik vektörü <strong>VQ (Vector Quantization)</strong> ve <strong>Enerji Karşılaştırma yöntemiyle</strong> sınıflandırılır ve veri tabanına ayrı ayrı kaydedilerek eğitim aşaması tamamlanır.</li>
<li>İkinci bölüm olan <strong>test</strong> aşamasında konuşmacı cinsiyeti bilinmeyen ses kayıtları giriş olarak alınır ve eğitim aşamasındaki gibi öznitelik vektörü hesaplanır. Elde edilen öznitelik vektörü eğitim veri tabanındaki verilerle kıyaslanarak erkek ve bayan sınıflar için ortalama bir uzaklık değeri hesaplanır. Bu uzaklık değerlerinden küçük olanı test verisinin hangi sınıfa ait olduğunu belirtir.</li>
</ul>
<p>Çalışmada, yaygın olarak kullanılan alınan <strong>.wav</strong> dosya formatındaki sesin byte byte işlenerek, analizi, bit kadar ince detaylarına girilmesi ve bunların çeşitli öznitelik çıkarmadan tutun da ön vurgulamaya kadar bu kısımlar üzerinde yoğun olarak çalışılmıştır. Sınıflandırma yöntemi olarak <strong>özdeğer vektörü</strong> çıkarma daha sonrasında <strong>ön-vurgulama (Pre-emphasis)</strong>, <strong>çerçeveleme</strong>, <strong>pencereleme</strong>, <strong>fast freguence transform(fft)</strong>, <strong>Mel spektrum</strong>, <strong>Mel Cepstrum</strong> konuları tek tek ele alınmıştır ve oluşturulan sisteme uygulanarak elde edilen konuşma sinyallerinin ayırımı, vektör haline getirilip <strong>VQ</strong> ve <strong>Enerji katsayıları</strong> kıyaslanıp, sınıflandırılarak başarılı bir şekilde gerçeklenmiştir</p>
<h2 id="kullanılan-yöntem-diagramları">Kullanılan Yöntem Diagramları</h2>
<p>Ses işleme akış diyagramı:</p>
<div class="mermaid"><svg xmlns="http://www.w3.org/2000/svg" id="mermaid-svg-FZx3gA0nJs9cm5ml" width="100%" style="max-width: 397.3125px;" viewBox="0 0 397.3125 542"><g transform="translate(-12, -12)"><g class="output"><g class="clusters"></g><g class="edgePaths"><g class="edgePath" style="opacity: 1;"><path class="path" d="M73.41201171875,260L138.109375,43L208.046875,43" marker-end="url(#arrowhead5222)" style="fill:none"></path><defs><marker id="arrowhead5222" viewBox="0 0 10 10" refX="9" refY="5" markerUnits="strokeWidth" markerWidth="8" markerHeight="6" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowheadPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"></path></marker></defs></g><g class="edgePath" style="opacity: 1;"><path class="path" d="M77.98356119791667,260L138.109375,139L218.1484375,139" marker-end="url(#arrowhead5223)" style="fill:none"></path><defs><marker id="arrowhead5223" viewBox="0 0 10 10" refX="9" refY="5" markerUnits="strokeWidth" markerWidth="8" markerHeight="6" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowheadPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"></path></marker></defs></g><g class="edgePath" style="opacity: 1;"><path class="path" d="M100.84130859375,260L138.109375,235L163.109375,235" marker-end="url(#arrowhead5224)" style="fill:none"></path><defs><marker id="arrowhead5224" viewBox="0 0 10 10" refX="9" refY="5" markerUnits="strokeWidth" markerWidth="8" markerHeight="6" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowheadPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"></path></marker></defs></g><g class="edgePath" style="opacity: 1;"><path class="path" d="M100.84130859375,306L138.109375,331L186.8828125,331" marker-end="url(#arrowhead5225)" style="fill:none"></path><defs><marker id="arrowhead5225" viewBox="0 0 10 10" refX="9" refY="5" markerUnits="strokeWidth" markerWidth="8" markerHeight="6" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowheadPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"></path></marker></defs></g><g class="edgePath" style="opacity: 1;"><path class="path" d="M77.98356119791667,306L138.109375,427L216.6171875,427" marker-end="url(#arrowhead5226)" style="fill:none"></path><defs><marker id="arrowhead5226" viewBox="0 0 10 10" refX="9" refY="5" markerUnits="strokeWidth" markerWidth="8" markerHeight="6" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowheadPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"></path></marker></defs></g><g class="edgePath" style="opacity: 1;"><path class="path" d="M73.41201171875,306L138.109375,523L243.203125,523" marker-end="url(#arrowhead5227)" style="fill:none"></path><defs><marker id="arrowhead5227" viewBox="0 0 10 10" refX="9" refY="5" markerUnits="strokeWidth" markerWidth="8" markerHeight="6" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowheadPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"></path></marker></defs></g></g><g class="edgeLabels"><g class="edgeLabel" transform="" style="opacity: 1;"><g transform="translate(0,0)" class="label"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;"><span class="edgeLabel"></span></div></foreignObject></g></g><g class="edgeLabel" transform="" style="opacity: 1;"><g transform="translate(0,0)" class="label"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;"><span class="edgeLabel"></span></div></foreignObject></g></g><g class="edgeLabel" transform="" style="opacity: 1;"><g transform="translate(0,0)" class="label"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;"><span class="edgeLabel"></span></div></foreignObject></g></g><g class="edgeLabel" transform="" style="opacity: 1;"><g transform="translate(0,0)" class="label"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;"><span class="edgeLabel"></span></div></foreignObject></g></g><g class="edgeLabel" transform="" style="opacity: 1;"><g transform="translate(0,0)" class="label"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;"><span class="edgeLabel"></span></div></foreignObject></g></g><g class="edgeLabel" transform="" style="opacity: 1;"><g transform="translate(0,0)" class="label"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;"><span class="edgeLabel"></span></div></foreignObject></g></g></g><g class="nodes"><g class="node" id="A" transform="translate(66.5546875,283)" style="opacity: 1;"><rect rx="0" ry="0" x="-46.5546875" y="-23" width="93.109375" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-36.5546875,-13)"><foreignObject width="73.109375" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">Ses İşleme</div></foreignObject></g></g></g><g class="node" id="B" transform="translate(282.2109375,43)" style="opacity: 1;"><rect rx="0" ry="0" x="-74.1640625" y="-23" width="148.328125" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-64.1640625,-13)"><foreignObject width="128.328125" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">1.wav Ses Dosyası</div></foreignObject></g></g></g><g class="node" id="C" transform="translate(282.2109375,139)" style="opacity: 1;"><rect rx="0" ry="0" x="-64.0625" y="-23" width="128.125" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-54.0625,-13)"><foreignObject width="108.125" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">2.Pre-Emphasis</div></foreignObject></g></g></g><g class="node" id="D" transform="translate(282.2109375,235)" style="opacity: 1;"><rect rx="0" ry="0" x="-119.1015625" y="-23" width="238.203125" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-109.1015625,-13)"><foreignObject width="218.203125" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">3.Çerçeveleme ve Pencereleme</div></foreignObject></g></g></g><g class="node" id="E" transform="translate(282.2109375,331)" style="opacity: 1;"><rect rx="0" ry="0" x="-95.328125" y="-23" width="190.65625" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-85.328125,-13)"><foreignObject width="170.65625" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">4.Fast Fourier Transform</div></foreignObject></g></g></g><g class="node" id="F" transform="translate(282.2109375,427)" style="opacity: 1;"><rect rx="0" ry="0" x="-65.59375" y="-23" width="131.1875" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-55.59375,-13)"><foreignObject width="111.1875" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">5.Mel Spectrum</div></foreignObject></g></g></g><g class="node" id="g" transform="translate(282.2109375,523)" style="opacity: 1;"><rect rx="0" ry="0" x="-39.0078125" y="-23" width="78.015625" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-29.0078125,-13)"><foreignObject width="58.015625" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">6.MFCC</div></foreignObject></g></g></g></g></g></g></svg></div>
<p>Eğitim akış diyagramı:</p>
<div class="mermaid"><svg xmlns="http://www.w3.org/2000/svg" id="mermaid-svg-XXHYlMvJE6C9Z3nR" width="100%" style="max-width: 327.296875px;" viewBox="0 0 327.296875 158"><g transform="translate(-12, -12)"><g class="output"><g class="clusters"></g><g class="edgePaths"><g class="edgePath" style="opacity: 1;"><path class="path" d="M79.451171875,68L109.1875,43L134.1875,43" marker-end="url(#arrowhead5235)" style="fill:none"></path><defs><marker id="arrowhead5235" viewBox="0 0 10 10" refX="9" refY="5" markerUnits="strokeWidth" markerWidth="8" markerHeight="6" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowheadPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"></path></marker></defs></g><g class="edgePath" style="opacity: 1;"><path class="path" d="M79.451171875,114L109.1875,139L134.765625,139" marker-end="url(#arrowhead5236)" style="fill:none"></path><defs><marker id="arrowhead5236" viewBox="0 0 10 10" refX="9" refY="5" markerUnits="strokeWidth" markerWidth="8" markerHeight="6" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowheadPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"></path></marker></defs></g></g><g class="edgeLabels"><g class="edgeLabel" transform="" style="opacity: 1;"><g transform="translate(0,0)" class="label"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;"><span class="edgeLabel"></span></div></foreignObject></g></g><g class="edgeLabel" transform="" style="opacity: 1;"><g transform="translate(0,0)" class="label"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;"><span class="edgeLabel"></span></div></foreignObject></g></g></g><g class="nodes"><g class="node" id="A" transform="translate(52.09375,91)" style="opacity: 1;"><rect rx="0" ry="0" x="-32.09375" y="-23" width="64.1875" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-22.09375,-13)"><foreignObject width="44.1875" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">Eğitim</div></foreignObject></g></g></g><g class="node" id="B" transform="translate(232.7421875,43)" style="opacity: 1;"><rect rx="0" ry="0" x="-98.5546875" y="-23" width="197.109375" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-88.5546875,-13)"><foreignObject width="177.109375" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">1.Kadın Seslerinin Eğitimi</div></foreignObject></g></g></g><g class="node" id="C" transform="translate(232.7421875,139)" style="opacity: 1;"><rect rx="0" ry="0" x="-97.9765625" y="-23" width="195.953125" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-87.9765625,-13)"><foreignObject width="175.953125" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">2.Erkek Seslerinin Eğitimi</div></foreignObject></g></g></g></g></g></g></svg></div>
<p>Sınıflandırma akış diyagramı:</p>
<div class="mermaid"><svg xmlns="http://www.w3.org/2000/svg" id="mermaid-svg-LB3owkU1yvoXxs6d" width="100%" style="max-width: 406.296875px;" viewBox="0 0 406.296875 158"><g transform="translate(-12, -12)"><g class="output"><g class="clusters"></g><g class="edgePaths"><g class="edgePath" style="opacity: 1;"><path class="path" d="M114.29248046875,68L156.296875,43L209.015625,43" marker-end="url(#arrowhead5244)" style="fill:none"></path><defs><marker id="arrowhead5244" viewBox="0 0 10 10" refX="9" refY="5" markerUnits="strokeWidth" markerWidth="8" markerHeight="6" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowheadPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"></path></marker></defs></g><g class="edgePath" style="opacity: 1;"><path class="path" d="M114.29248046875,114L156.296875,139L181.296875,139" marker-end="url(#arrowhead5245)" style="fill:none"></path><defs><marker id="arrowhead5245" viewBox="0 0 10 10" refX="9" refY="5" markerUnits="strokeWidth" markerWidth="8" markerHeight="6" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowheadPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"></path></marker></defs></g></g><g class="edgeLabels"><g class="edgeLabel" transform="" style="opacity: 1;"><g transform="translate(0,0)" class="label"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;"><span class="edgeLabel"></span></div></foreignObject></g></g><g class="edgeLabel" transform="" style="opacity: 1;"><g transform="translate(0,0)" class="label"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;"><span class="edgeLabel"></span></div></foreignObject></g></g></g><g class="nodes"><g class="node" id="A" transform="translate(75.6484375,91)" style="opacity: 1;"><rect rx="0" ry="0" x="-55.6484375" y="-23" width="111.296875" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-45.6484375,-13)"><foreignObject width="91.296875" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">Sınıflandırma</div></foreignObject></g></g></g><g class="node" id="B" transform="translate(295.796875,43)" style="opacity: 1;"><rect rx="0" ry="0" x="-86.78125" y="-23" width="173.5625" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-76.78125,-13)"><foreignObject width="153.5625" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">1.Vector Quantızatıon</div></foreignObject></g></g></g><g class="node" id="C" transform="translate(295.796875,139)" style="opacity: 1;"><rect rx="0" ry="0" x="-114.5" y="-23" width="229" height="46"></rect><g class="label" transform="translate(0,0)"><g transform="translate(-104.5,-13)"><foreignObject width="209" height="26"><div xmlns="http://www.w3.org/1999/xhtml" style="display: inline-block; white-space: nowrap;">2.Enerji Karşılaştırma Yöntemi</div></foreignObject></g></g></g></g></g></g></svg></div>

    </div>
  </div>
</body>

</html>
