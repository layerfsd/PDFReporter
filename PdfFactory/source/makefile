
CFLAGS = -I. -g -Wall -DLINUX_VERSION -I../depending/libxml2-2.6.27/include \
-I./pdfgenerator
LIBS = -lm

TEST_PDF = ../windlltest/Main.o PdfBaseObject.o \
NumberObject.o DLList.o PdfDocument.o Rectangle.o PdfContentStream.o \
PdfCatalog.o CrossReferenceTable.o DictionaryObject.o NameObject.o \
IndirectReference.o PdfPages.o StreamObject.o ArrayObject.o PdfPage.o \
PdfHeader.o PdfTrailer.o PdfOutlines.o StringObject.o \
IndirectObject.o PdfFont.o PdfPageResources.o \
pdfgenerator/PdfGeneratorDataStream.o PdfTextWriter.o \
pdfgenerator/PdfGeneratorDataStreamRow.o UnitConverter.o \
pdfgenerator/PdfGeneratorDataStreamColumn.o \
pdfgenerator/PdfGeneratorDataStreamField.o

TARGETS = testPdf

all: $(TARGETS)

testPdf: $(TEST_PDF)
	gcc -o testPdf $(TEST_PDF) $(LIBS)

%.o: %.c
	gcc -c $(CFLAGS) $< -o $@

clean:
	-rm -vf *core $(TARGETS) *.o ../windlltest/*.o test*.pdf
