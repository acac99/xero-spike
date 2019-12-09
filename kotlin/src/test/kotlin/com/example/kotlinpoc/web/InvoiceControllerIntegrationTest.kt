package com.example.kotlinpoc.web

import com.example.kotlinpoc.persistence.Invoice
import com.example.kotlinpoc.persistence.InvoiceRepository
import com.example.kotlinpoc.persistence.LineItem
import org.assertj.core.api.Assertions.assertThat
import org.junit.jupiter.api.Test
import org.skyscreamer.jsonassert.JSONAssert
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.boot.test.web.client.TestRestTemplate
import org.springframework.boot.test.web.client.getForEntity
import org.springframework.http.HttpStatus
import java.math.BigDecimal
import java.util.*


@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
class IntegrationTests(@Autowired val restTemplate: TestRestTemplate, @Autowired val invoiceRepository: InvoiceRepository) {


    @Test
    fun `Expect GET invoice to return invoices`() {
        //when
        val entity = restTemplate.getForEntity<String>("/invoice")

        //then
        assertThat(entity.statusCode).isEqualTo(HttpStatus.OK)
        JSONAssert.assertEquals(entity.body, "[]", true)

    }

    @Test
    fun `Expect GET invoice{id} to return 404 if invoice not found`() {
        //given
        val params = mapOf(Pair("id", UUID.randomUUID()))

        //when
        val entity = restTemplate.getForEntity<String>("/invoice/{id}", params)

        //then
        assertThat(entity.statusCode).isEqualTo(HttpStatus.NOT_FOUND)
    }


    @Test
    fun `Expect GET invoice{id} to return invoice`() {
        //given
        val invoice = Invoice(
                status = "pending",
                amountDue = BigDecimal("120.00"),
                lineItems = listOf(LineItem(
                        description = "bla",
                        quantity = 2.0,
                        unitAmount = BigDecimal("60"),
                        lineAmount = BigDecimal("120"),
                        invoice = null
                ))
        )
        val savedInvoice = invoiceRepository.save(invoice)
        val params = mapOf(Pair("id", savedInvoice.id))

        //when
        val entity = restTemplate.getForEntity<String>("/invoice/{id}", params)

        //then
        assertThat(entity.statusCode).isEqualTo(HttpStatus.OK)
//        JSONAssert.assertEquals( "{}", entity.body, true)
    }

}