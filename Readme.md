# Moogle!

![](moogle.png)

>   Osvaldo R. Moreno P. C111. Proyecto de Programación I. Facultad de Matemática y Computación. Universidad de La Habana. Curso 2022. 

# Cómo funciona el Moogle???

**Sobre la parte visual:** Se ha comprobado que en algunos navegadores como FireFox, no carga como debería ser, recomendamos usar Chrome para la ejecución.
**A tener en cuenta:** La Carpeta Content debe esta en la "ubicación moogle-2021-main/", en caso de no aparecer saltaria una Advertencia en Consola, podemos cambiar la ubicación de la misma en la clase Folder; además, debe contener al menos un documento, en caso contrario aparecera una Advertencia en Consola. 

## Sobre el Pre-Procesamiento:

(El método encargado, se ejecuta antes de correr la app para tener precalculados los datos necesarios)

Lo primero es procesar todos los documentos de la carpeta Content, normalizando sus textos (eliminando los signos de puntuación y eliminando en sus palabras, caracteres extraños que tengan en su composición), y guardando por documento cada palabra del texto, la cantidad de veces que se repite (TF- frecuencia del termino), la cantidad de palabras del mismo, y  además la cantidad de documentos que contienen una palabra especifica (IDF).

Luego calculamos el TF-IDF de cada palabra por documento con la fórmula:

TF-IDF <sub>(palabra)</sub>  = (TF <sub>(palabra)</sub> / Cantidad de Palabras<sub>(texto)</sub>) * log ( IDF<sub>(palabra)</sub>/ Cantidad de Documentos)

En caso de solo haber un documento, nos quedamos solo con el TF, ya que:

(IDF<sub>(palabra)</sub>/ Cantidad de Documentos) = 1 => log 1 = 0, y nos anularía el TF-IDF de la palabra, lo cual no queremos.

Con esto ya tenemos creadas las bases para poder proceder al trabajo con la query.

## Sobre el Procesamiento de la Query:

Al igual que en el procesamiento de los documentos, la query la tratamos como un pseudo-documento, normalizamos, calculamos su TF, y recogemos sus operadores:

(Debido a que los operadores en la orden del Moogle, * ! ^ se escribian sin espaciado, y el operador ~ se escribia con espaciado entre las palabras, he decidido recibir sin espacidado todos los operadores para reutilizar los Metodos en caso de ser necesario)

### Sobre el uso de los Operadores:

~ **Operador de Cercanía:** Se coloca entre las palabras que queremos que estén cercas, sin espaciado de por medio ( este~es~un~ejemplo sobre~como funciona), busca entre dos o mas palabras anidadas, y entre palabras distintas del documento, se le asigna un valor de 1/(distancia entre las palabras), de manera que las palabras que estén muy separadas tengan un score casi mínimo.

´*´ **Operador de Importancia:** Se colora ante la palabra, sin espaciado, y se repite según la cantidad de importancia que queremos asignarle, se le da un valor de 0.25 por cada * ante la palabra.

! **Operador de No Debe aparecer:** Se coloca ante la palabra, sin espaciado, eliminando los documentos que contengan la palabra precedida.

^ **Operador de Debe aparecer:** Se coloca ante la palabras, sin espaciado, dejando solo los documentos que deban contener la palabra precedida.

## Sobre la selección de Documentos relacionados a la query:

Para la selección, llevamos a nuestros documentos a vectores (de manera que sus componentes son la palabras del documento, y su valor TF-IDF), luego de tener nuestro modelo, calculamos el coseno con respecto al vector query mediante el [Dot Product](VectorSpaceModel.odg) (abajo tenemos el documento), esto nos permite ganar en tiempo, ya que calculamos el coseno recorriendo solo las palabras comunes del texto con repecto a la query, y evitar calcular los  módulos y ángulos entre los vectores.


Luego organizamos los vectores, de manera decreciente según su coseno, de manera que a mayor coseno es más relacionado a la query, el valor del coseno varia de 1 a 0, siendo 1 un documento que contenga completamente la query, y 0 un documento no relacionado con la búsqueda.

## Sobre el Snippet:

Para la elección del mejor Snippet hemos escogido el subtexto que contenga mayor cantidad de palabras de la query, enfocandonos en el TF de las palabras en el subtexto, como un Subarray de suma máxima.

## Sobre la Sugerencia:

Para la sugerencia escogemos las palabras de la búsqueda del usuario que no aparecen, luego buscamos en los textos la palabra más parecida a la que tenemos, (definimos palabra parecida, a una palabra que tenga a lo sumo la mitad de caracteres distintos).
Luego construimos nuestra sugerencia con las palabras recibidas, en caso que se hayan encontrado palabras parecidas, y lo mostramos como sugerencia. Al hacer clic en esta, Moogle la buscará reemplazando la query original.


Mucho gusto🖖