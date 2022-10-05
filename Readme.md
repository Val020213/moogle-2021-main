# Moogle!

![](moogle.png)

>   Osvaldo R. Moreno P. C111. Proyecto de Programación I. Facultad de Matemática y Computación. Universidad de La Habana. Curso 2022. 

# Actualizaciones del Moogle!!!
 ** hola de vuelta ...** traemos actualizaciones!, que **no Sabes qué es Moogle!?**, no te preocupes
 puedes leer sobre Moogle [aqui](ReadmeMoogleInfo.md).


## Actualización del Snippet:
El Snippet anterior procesaba sobre el texto modificado, es decir, 0 signos de puntuación, 0 mayúsculas, todo esto sacrificado por una forma rápida de procesar, pero con el nuevo Método implementado ya se sobre el texto original! e igual de rápido. Además se tienen en cuenta aspectos como la cantidad de palabras distintas de la query en el Snippet, ganando menos bonus un Pre-Snippet si contiene solo las mismas palabras pero repetidas, brindando un Snippet más completo; se ha configurado la relevancia de los operadores en la búsqueda del mejor Snippet, de manera que un Pre-Snippet con palabras incluidas en el operador importancia, gana un bonus extra.
 
## Sobre el Apartado Visual:
Ahora se muestran la cantidad de elementos relacionados con la búsqueda, y el tiempo en que se realizó la misma.

## MoogleEngine Code:
Se movieron los Métodos get_next_word y get_previous_words de la clase Query a StringMethods, por organización y estructura de los nuevos métodos implementados

# Proyecciones para las próximas actualizaciones:
Se piensa implementar una forma de acceso de los resultados de la búsqueda a sus documentos respectivos mediante hipervinculos (fase beta actualmente).
Marcar las palabras claves de los Snippets de la búsqueda.


Eso es todo por ahora, Chao, nos vemos en la próxima actualización 🖖